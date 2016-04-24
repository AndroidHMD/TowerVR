using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public sealed class LocalTowerGameManagerImpl : TowerGameManagerImpl
    {       
        #region PROTECTED_MEMBER_FUNCTIONS
        
        protected override void _onEventHandler(byte eventCode, object content, int senderId)
        {
            
        }
        
        protected override void _notifyIsReady(int playerID)
        {
            Log("notifyIsReady [playerID=" + playerID + "]");
            
            //todo
        }
        
        protected override void _tryStartGame()
        {
            Log("tryStartGame");
            
            //todo
        }
        
        void Awake()
        {	
			gameState = GameState.GAME_AWAITING_PLAYERS;
			turnState = TurnState.TURN_NOT_STARTED;
			
			players = new HashSet<PhotonPlayer>();
			playersReadyMap = new Dictionary<PhotonPlayer, bool>();
			
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				players.Add(photonPlayer);
				playersReadyMap[photonPlayer] = false;
			}
        }
        
        #endregion PROTECTED_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
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
        
        private void notifyAllPlayersReady()
        {
            
        }
        
        private static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
        
        
        #region PRIVATE_MEMBER_VARIABLES
        
        private GameState gameState;
        
        private TurnState turnState;
        
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