using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public sealed class MasterTowerGameManagerImpl : TowerGameManagerImpl
    {      
        #region PUBLIC_MEMBER_FUNCTIONS
		
		public sealed override void notifyIsReady()
		{
			handlePlayerReadyEvent(PhotonNetwork.player.ID);
		}
        
        public sealed override void tryStartGame()
		{
			handleTryStartGameEvent(PhotonNetwork.player.ID);
		}
		
		public sealed override void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY)
		{
			handlePlaceTowerPieceEvent(PhotonNetwork.player.ID, positionX, positionZ, rotationDegreesY);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
        
        
        
         
        #region PROTECTED_MEMBER_FUNCTIONS
        
        protected sealed override void Awake()
        {	
            base.Awake();
            
			gameState = GameState.AwaitingPlayers;
			turnState = TurnState.NotStarted;
			
			players = new HashSet<PhotonPlayer>();
			playersReadyMap = new Dictionary<PhotonPlayer, bool>();
			
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				players.Add(photonPlayer);
				playersReadyMap[photonPlayer] = false;
			}
        }
        
        protected sealed override void onEvent(byte eventCode, object content, int senderID)
		{
            base.onEvent(eventCode, content, senderID);
            
			switch (eventCode)
			{
				case NetworkEventCodes.PlayerReady:
				{
					handlePlayerReadyEvent(senderID); 
					break;	
				}
				
				case NetworkEventCodes.TryStartGame:
				{
					handleTryStartGameEvent(senderID); 
					break;	
				}
				
				case NetworkEventCodes.PlaceTowerPiece:
				{
					float posX, posZ, rotDegreesY;
                    if (PlaceTowerPieceEvent.TryParse(content, out posX, out posZ, out rotDegreesY))
                    {
                        handlePlaceTowerPieceEvent(posX, posZ, rotDegreesY);
                    }
                    else
                    {
                        LogMalformedEventContent("PlaceTowerPieceEvent", senderID);
                    }
					break;	
				}
				
				default:
					return;
			}
		}
        
        //////////////////////////////////
		/// PhotonNetworkEvent handles ///
		//////////////////////////////////
        
        protected void handlePlayerReadyEvent(int playerID)
        {
            Log("handlePlayerReadyEvent");
            
            foreach (var player in playersReadyMap)
            {
                if (player.Key.ID == playerID)
                {
                    // Mark that player as ready
                    playersReadyMap[player.Key] = true;
                }
            }
            
            if (allPlayersReady())
            {
                gameState = GameState.AllPlayersReady;
                syncGameState();
            }
        }
        
		protected void handleTryStartGameEvent(int playerID)
        {
            Log("handleTryStartGameEvent");
            
            if (allPlayersReady())
            {
                gameState = GameState.Running;
                syncGameState();
            }
        }
        
		protected void handlePlaceTowerPieceEvent(int playerID, float posX, float posZ, float rotDegreesY)
        {
            Log("handlePlaceTowerPieceEvent [playerID=" + playerID + " posX=" + posX + " posZ=" + posZ + "rotDegreesY=" + rotDegreesY + "]");
        }
        
        
        #endregion PROTECTED_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
        private void syncGameState()
        {
            var ev = new GameStateChangedEvent(gameState);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
            }
        }
        
        private void syncTurnState()
        {
            var ev = new TurnStateChangedEvent(turnState);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
            }
        }
        
        /**
		 * Checks if all players are ready to start the game.
		 * 
		 * Returns true if #reportIsReady has been called with each PhotonPlayer as argument.
		 * */ 
		private bool allPlayersReady()
		{
			foreach (var entry in playersReadyMap)
			{
				if (entry.Value == false)
				{
					return false;
				}
			}
			
			return true;
		}
        
        private static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        private static void Error(object obj)
        {
            Debug.LogError(obj.ToString());
        }
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
        
        
        #region PRIVATE_MEMBER_VARIABLES
        
        private int gameState 
        { 
            get { return gameState; }
            set { if (GameState.IsValid(value)) gameState = value; }
        }
        
        private int turnState 
        { 
            get { return turnState; }
            set { if (TurnState.IsValid(value)) turnState = value; } 
        }
        
        private HashSet<PhotonPlayer> players;
        
        private IDictionary<PhotonPlayer, bool> playersReadyMap;
        
        /**
		 * A queue containing the next players.
		 * 
		 * When the current player's turn is done, the next player is taken from the front of the queue.
		 * 
		 * If the current player performs an action that succeeds, the player is appended to the queue again.
		 * If the current player performs an action that causes the tower to fall, 
		 * 		the player is not appended to the queue again.
		 * */
        private Queue<PhotonPlayer> playerQueue;
        
        #endregion PRIVATE_MEMBER_VARIABLES
    }
}