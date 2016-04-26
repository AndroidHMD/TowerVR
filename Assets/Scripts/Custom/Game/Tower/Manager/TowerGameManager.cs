using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public class TowerGameManager : Singleton<TowerGameManager>, ITowerGameManager
	{		
		#region PUBLIC_MEMBER_FUNCTIONS
		
		public void notifyIsReady()
		{
			impl.notifyIsReady();
		}
        
        public void tryStartGame()
		{
			impl.tryStartGame();
		}
		
		public void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY)
		{
			impl.placeTowerPiece(positionX, positionZ, rotationDegreesY);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		
		
		
		#region DELEGATES
		
		public delegate void GameStateChangedHandler(int gameState);
		public HashSet<GameStateChangedHandler> gameStateChangedHandlers = new HashSet<GameStateChangedHandler>();
		
		public delegate void TurnStateChangedHandler(int turnState);
		public HashSet<TurnStateChangedHandler> turnStateChangedHandlers = new HashSet<TurnStateChangedHandler>();
		
		public delegate void PlayerLostHandler(int losingPlayerID);
		public HashSet<PlayerLostHandler> playerLostHandlers = new HashSet<PlayerLostHandler>();
		
		public delegate void PlayerWonHandler(int winningPlayerID);
		public HashSet<PlayerWonHandler> playerWonHandlers = new HashSet<PlayerWonHandler>();
		
		public delegate void NextPlayerTurnHandler(int nextPlayerID);
		public HashSet<NextPlayerTurnHandler> nextPlayerTurnHandlers = new HashSet<NextPlayerTurnHandler>();
		
		public delegate void ScoreUpdatedHandler(int playerID, Score score);
		public HashSet<ScoreUpdatedHandler> scoreUpdatedHandlers = new HashSet<ScoreUpdatedHandler>();
		
		#endregion DELEGATES


		#region PRIVATE_MEMBERS
		
		void Awake()
		{	
			if (PhotonNetwork.isMasterClient)
			{
				Debug.Log("Player is master client, instantiating MasterTowerGameManagerImpl");
				impl = gameObject.AddComponent<MasterTowerGameManagerImpl>();
			}
			
			else
			{
				Debug.Log("Player is NOT master client, instantiating TowerGameManagerImpl");
				impl = gameObject.AddComponent<TowerGameManagerImpl>();
			}
			
			impl.parent = this;
		}
		
		private TowerGameManagerImpl impl;	
		
		#endregion PRIVATE_MEMBERS

	}
}