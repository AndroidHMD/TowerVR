using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TowerVR
{
	public class TowerGameManagerImpl : Singleton<TowerGameManagerImpl>, TowerGameManager
	{		
		#region PUBLIC_MEMBER_FUNCTIONS
		
		public void getGameState(Action<GameState> callback)
		{
			impl.getGameState(callback);
		}
        
        public void getTurnState(Action<TurnState> callback)
		{
			impl.getTurnState(callback);
		}
        
        public void notifyIsReady(int playerID)
		{
			impl.notifyIsReady(playerID);
		}
        
        public void tryStartGame(Action<bool> callback)
		{
			impl.tryStartGame(callback);
		}
        
        public void tryGetScore(int playerID, Action<Score> callback)
		{
			impl.tryGetScore(playerID, callback);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		
		
		void Awake()
		{	
			if (PhotonNetwork.isMasterClient)
			{
				impl = new LocalTowerGameManager();
			}
			
			else
			{
				impl = new RemoteTowerGameManager();
			}
		}


		#region PRIVATE_MEMBER_VARIABLES
		
		private TowerGameManager impl;		
		
		#endregion PRIVATE_MEMBER_VARIABLES

	}
}