using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	/**
	 * Implementation of the ITowerGameManager interface.
	 * 
	 * Attach this component to EXACTLY ONE gameobject in the tower game scene.
	 **/
	public sealed class TowerGameManager : Singleton<TowerGameManager>, ITowerGameManager
	{		
		#region PUBLIC_MEMBER_FUNCTIONS
		
		/// See ITowerGameManager
		public void notifyIsReady()
		{
			impl.notifyIsReady();
		}
        
		/// See ITowerGameManager
        public void tryStartGame()
		{
			impl.tryStartGame();
		}
		
		/// See ITowerGameManager
		public void selectTowerPiece(TowerPieceDifficulty difficulty)
		{
			impl.selectTowerPiece(difficulty);
		}
		
		/// See ITowerGameManager
		public void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY)
		{
			impl.placeTowerPiece(positionX, positionZ, rotationDegreesY);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		
		
		
		#region DELEGATES
		
		////////////////////////////////////////////////////////////////////////////////
		/// Subscribe to these delegates to receive game logic updates:
		/// 1. Create a method with the corresponding '*Handler*' signature.
		/// 2. Call '*Handlers'.Add([your method here])
		/// 3. Win
		/// End. Call '*Handlers'.Remove([your method here]) in the script's OnDestroy
		////////////////////////////////////////////////////////////////////////////////
		
		public delegate void GameStateChangedHandler(int gameState);
		public HashSet<GameStateChangedHandler> gameStateChangedHandlers = new HashSet<GameStateChangedHandler>();
		
		public delegate void TurnStateChangedHandler(int turnState);
		public HashSet<TurnStateChangedHandler> turnStateChangedHandlers = new HashSet<TurnStateChangedHandler>();

		public delegate void TowerStateChangedHandler(int towerState);
		public HashSet<TowerStateChangedHandler> towerStateChangedHandlers = new HashSet<TowerStateChangedHandler>();
		
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
			// Instantiate an implementation of the correct type
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
		
		/**
		 * The underlying implementation
		 **/
		private TowerGameManagerImpl impl;	
		
		#endregion PRIVATE_MEMBERS

	}
}