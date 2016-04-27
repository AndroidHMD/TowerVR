using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    /**
     * An ITowerGameManager that represents the master client's instance. This keeps track of the
     * state as well.
     **/ 
    public sealed class MasterTowerGameManagerImpl : TowerGameManagerImpl
    {      
        #region PUBLIC_MEMBER_FUNCTIONS
		
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
		public sealed override void notifyIsReady()
		{
			handlePlayerReadyEvent(PhotonNetwork.player.ID);
		}
        
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
        public sealed override void tryStartGame()
		{
			handleTryStartGameEvent(PhotonNetwork.player.ID);
		}
		
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
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
			towerState = TowerState.Stationary;
			
			players = new HashSet<PhotonPlayer>();
			playersReadyMap = new Dictionary<PhotonPlayer, bool>();
			
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				players.Add(photonPlayer);
				playersReadyMap.Add(photonPlayer, false);
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
                        handlePlaceTowerPieceEvent(senderID, posX, posZ, rotDegreesY);
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
            
            PhotonPlayer photonPlayer;
            if (tryGetPhotonPlayer(playerID, out photonPlayer))
            {
                playersReadyMap[photonPlayer] = true;
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
                initPlayerTurnQueue();
                proceedPlayerTurn();
                
                // todo remove this once testing is finished
                InvokeRepeating("proceedPlayerTurn", 5, 5);
                
                syncGameState();
            }
        }
        
		protected void handlePlaceTowerPieceEvent(int playerID, float posX, float posZ, float rotDegreesY)
        {
            Log("handlePlaceTowerPieceEvent [playerID=" + playerID + " posX=" + posX + " posZ=" + posZ + "rotDegreesY=" + rotDegreesY + "]");
        }
        
        
        #endregion PROTECTED_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
        /**
         * Sync the game state with all clients.
         **/
        private void syncGameState()
        {
            var ev = new GameStateChangedEvent(gameState);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
            }
        }
        
        /**
         * Sync the turn state with all clients.
         **/
        private void syncTurnState()
        {
            var ev = new TurnStateChangedEvent(turnState);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
            }
        }

		/**
         * Sync the tower state with all clients.
         **/
		private void syncTowerState()
		{
			var ev = new TowerStateChangedEvent(towerState);
			if (!ev.trySend())
			{
				Error(ev.trySendError);
			}
		}
        
        /**
         * Initializes the player turn queue with the players that were online on scene startup.
         **/
        private void initPlayerTurnQueue()
        {
            playerQueue = new Queue<PhotonPlayer>();
            
            foreach (var photonPlayer in players)
            {
                playerQueue.Enqueue(photonPlayer);
            }
        }
        
        /**
         * Ends the current player's turn and continues to the next player.
         **/
        private void proceedPlayerTurn()
        {
            PhotonPlayer lastPhotonPlayer = currentPlayer;
            currentPlayer = null;
            
            if (lastPhotonPlayer != null)
            {
                playerQueue.Enqueue(lastPhotonPlayer);
            }
            
            var nextPlayer = playerQueue.Dequeue();
            
            var ev = new NextPlayerEvent(nextPlayer.ID);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
                gameState = GameState.Stopped;
                syncGameState();
                return;
            }
            
            currentPlayer = nextPlayer;
            
            turnState = TurnState.SelectingTowerPiece;
            syncTurnState();
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
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
        
        
        #region PRIVATE_MEMBER_VARIABLES
        
        // The game state.
		private int gameState;
        
        // The turn state.
		private int turnState;

		// The tower state
		private int towerState;
        
        // The players that we're in the room when the manager was instantiated.
        private HashSet<PhotonPlayer> players;
        
        // Flags to see if each player is online and ready.
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
        
        // Reference to the current player.
        private PhotonPlayer currentPlayer;
        
        #endregion PRIVATE_MEMBER_VARIABLES
        
        /**
         * Helper to retrieve a PhotonPlayer instance based on an ID.
         **/
        private bool tryGetPhotonPlayer(int playerID, out PhotonPlayer photonPlayer)
        {
            foreach (var _photonPlayer in players)
            {
                if (_photonPlayer.ID == playerID)
                {
                    photonPlayer = _photonPlayer;
                    return true;
                }
            }
            
            photonPlayer = null;
            return false;
        }
        
        private static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        private static void Error(object obj)
        {
            Debug.LogError(obj.ToString());
        }
    }
}