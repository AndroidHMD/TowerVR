using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public abstract class TowerGameManagerImpl : Singleton<TowerGameManagerImpl>, ITowerGameManager
	{		
		#region PUBLIC_MEMBER_FUNCTIONS
		
		public void notifyIsReady(int playerID)
		{
			_notifyIsReady(playerID);
		}
        
        public void tryStartGame()
		{
			_tryStartGame();
		}
		
        public void addListener(ITowerGameManagerListener listener)
		{
			listeners.Add(listener);
		}
        
        public void removeListener(ITowerGameManagerListener listener)
		{
			listeners.Remove(listener);
		}
		
		void Awake()
		{
			PhotonNetwork.OnEventCall += _onEventHandler;
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		

		#region ABSTRACT_MEMBER_FUNCTIONS
		
		protected abstract void _onEventHandler(byte eventCode, object content, int senderId);
		
		protected abstract void _notifyIsReady(int playerID);
		
		protected abstract void _tryStartGame();
		
		#endregion ABSTRACT_MEMBER_FUNCTIONS


		#region PRIVATE_MEMBER_VARIABLES
		
		private IList<ITowerGameManagerListener> listeners = new List<ITowerGameManagerListener>();
		
		#endregion PRIVATE_MEMBER_VARIABLES

	}
}