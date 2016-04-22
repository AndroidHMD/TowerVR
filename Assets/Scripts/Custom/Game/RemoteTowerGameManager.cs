using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public class RemoteTowerGameManager : MonoBehaviour, TowerGameManager
    {       
        #region PUBLIC_MEMBER_FUNCTIONS
         
        public void getGameState(Action<GameState> callback)
        {
            Log("getGameState()");
            
            // todo
        }
        
        public void getTurnState(Action<TurnState> callback)
        {
            Log("getTurnState");
            
            // todo
        }
        
        public void notifyIsReady(int playerID)
        {
            Log("notifyIsReady [playerID=" + playerID + "]");
            
            // todo
        }
        
        public void tryStartGame(Action<bool> callback)
        {
            Log("tryStartGame");
            
            // todo
        }
        
        /**
		 * (Tries to) get the score of the given player.
		 * */
        public void tryGetScore(int playerID, Action<Score> callback)
        {
            Log("tryGetScore [playerID=" + playerID + "]");
            
            // todo
        }
        
        #endregion PUBLIC_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
        
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