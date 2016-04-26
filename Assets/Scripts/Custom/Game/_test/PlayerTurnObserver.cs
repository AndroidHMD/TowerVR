using UnityEngine;
using System.Collections;

namespace TowerVR
{
	public class PlayerTurnObserver : MonoBehaviour 
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
			
			TowerGameManager.Instance.gameStateChangedHandlers.Add(onGameStateChanged);
			TowerGameManager.Instance.nextPlayerTurnHandlers.Add(onNextPlayerTurn);
			TowerGameManager.Instance.notifyIsReady();
		}
		
		void Update()
		{
			if (Input.anyKeyDown || Input.touchCount > 0)
			{
				if (gameState == GameState.AwaitingPlayers)
				{
					TowerGameManager.Instance.notifyIsReady();
				}
				
				else
				{
					TowerGameManager.Instance.tryStartGame();
				}
			}
		}
		
		void OnDestroy()
		{
			TowerGameManager.Instance.gameStateChangedHandlers.Remove(onGameStateChanged);
			TowerGameManager.Instance.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
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