using UnityEngine;
using System.Collections;

namespace TowerVR
{
	public class PlayerTurnObserver : TowerVR.TowerGameBehaviour 
	{
		private int gameState;
		private int currentPlayerID;
		
		void onGameStateChanged(int gameState)
		{
			this.gameState = gameState;
		}
		
		void onNextPlayerTurn(int nextPlayerID)
		{
			this.currentPlayerID = nextPlayerID;
		}
		
		void Start () 
		{
			gameState = GameState.AwaitingPlayers;
			
			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Add(onNextPlayerTurn);
			manager.notifyIsReady();
		}
		
		void Update()
		{
			if (Input.anyKeyDown || Input.touchCount > 0)
			{
				if (gameState == GameState.AwaitingPlayers)
				{
					manager.notifyIsReady();
				}
				
				else
				{
					manager.tryStartGame();
				}
			}
		}
		
		void OnDestroy()
		{
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
		}
		
		void OnGUI()
		{
			GUILayout.Label("GameState=" + gameState.ToString());
			GUILayout.Label("Current player ID=" + currentPlayerID.ToString());
			GUILayout.Label("Your player ID=" + PhotonNetwork.player.ID.ToString());
			GUILayout.Label("Player count=" + PhotonNetwork.playerList.Length);
			
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
				GUILayout.Label("It is your turn!");
			}
		}
	}
}