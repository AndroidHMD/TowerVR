using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public class LocalTowerGameManager : MonoBehaviour, TowerGameManager
    {       
        #region PUBLIC_MEMBER_FUNCTIONS
         
        public void getGameState(Action<GameState> callback)
        {
            Log("getGameState()");
            callback(gameState);
        }
        
        public void getTurnState(Action<TurnState> callback)
        {
            Log("getTurnState");
            callback(turnState);
        }
        
        public void notifyIsReady(int playerID)
        {
            Log("notifyIsReady [playerID=" + playerID + "]");
/*
            for
            if (playersReadyMap.ContainsKey(player))
			{
				playersReadyMap[player] = true;	
			}
			
			if (allPlayersReady())
			{
				gameState = GameState.GAME_PREPARING_START;
			}
*/
        }
        
        public void tryStartGame(Action<bool> callback)
        {
            Log("tryStartGame");
        }
        
        /**
		 * (Tries to) get the score of the given player.
		 * */
        public void tryGetScore(int playerID, Action<Score> callback)
        {
            Log("tryGetScore [playerID=" + playerID + "]");
/*
            Score score;
            
            if (players.Contains(player))
            {
                score = players
            }
*/
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
        
        #endregion PUBLIC_MEMBER_FUNCTIONS
        
        
        
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