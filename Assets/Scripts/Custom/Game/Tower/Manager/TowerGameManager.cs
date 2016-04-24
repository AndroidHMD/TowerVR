using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public class TowerGameManager : Singleton<TowerGameManager>, ITowerGameManager
	{		
		#region PUBLIC_MEMBER_FUNCTIONS
		
		public void notifyIsReady(int playerID)
		{
			impl.notifyIsReady(playerID);
		}
        
        public void tryStartGame()
		{
			impl.tryStartGame();
		}
		
        public void addListener(ITowerGameManagerListener listener)
		{
			impl.addListener(listener);
		}
        
        public void removeListener(ITowerGameManagerListener listener)
		{
			impl.removeListener(listener);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		
		
		void Awake()
		{	
			if (PhotonNetwork.isMasterClient)
			{
				Debug.Log("Player is master client, instantiating LocalTowerGameManagerImpl");
				impl = gameObject.AddComponent<LocalTowerGameManagerImpl>();
			}
			
			else
			{
				Debug.Log("Player is NOT master client, instantiating RemoteTowerGameManagerImpl");
				impl = gameObject.AddComponent<RemoteTowerGameManagerImpl>();
			}
		}


		#region PRIVATE_MEMBER_VARIABLES
		
		private TowerGameManagerImpl impl;	
		
		#endregion PRIVATE_MEMBER_VARIABLES

	}
}