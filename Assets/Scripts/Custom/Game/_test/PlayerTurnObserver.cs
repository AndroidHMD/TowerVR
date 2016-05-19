using UnityEngine;
using System.Collections;

namespace TowerVR
{
	public class PlayerTurnObserver : TowerVR.TowerGameBehaviour 
	{
		private int turnState;
		private int gameState;
		private int currentPlayerID;
		private bool hasReadied;
		
		private string winString = "";
		
		void onGameStateChanged(int gameState)
		{
			this.gameState = gameState;
		}
		
		void onTurnStateChanged(int turnState)
		{
			this.turnState = turnState;
		}
		
		void onNextPlayerTurn(int nextPlayerID)
		{
			this.currentPlayerID = nextPlayerID;
		}
		
		void onPlayerWon(int playerID)
		{
			if (playerID == PhotonNetwork.player.ID)
			{
				winString = "You won! Gratz m8!";
			}
			
			else
			{
				winString = "You suck, you lost!";
			}
		}
		
		void Start () 
		{
			hasReadied = false;
			
			turnState = TurnState.NotStarted;
			gameState = GameState.AwaitingPlayers;
			
			manager.turnStateChangedHandlers.Add(onTurnStateChanged);
			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Add(onNextPlayerTurn);
			
			manager.playerWonHandlers.Add(onPlayerWon);
		}
		
		void OnDestroy()
		{
			manager.turnStateChangedHandlers.Remove(onTurnStateChanged);
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
			
			manager.playerWonHandlers.Remove(onPlayerWon);
		}
		
		void OnGUI()
		{
			GUILayout.Label("");
			GUILayout.Label("Player count: " + PhotonNetwork.playerList.Length);
			
			GUILayout.Label("GameState: " + GameState.ToString(gameState));
			GUILayout.Label("TurnState: " + TurnState.ToString(turnState));
			
			if (gameState == GameState.AwaitingPlayers && !hasReadied)
			{
				if (GUILayout.Button("Ready"))
				{
					hasReadied = true;
					manager.notifyIsReady();
				}
			}
			
			if (gameState == GameState.AllPlayersReady)
			{
				if (GUILayout.Button("Start game"))
				{
					manager.tryStartGame();
				}
			}
			
			if (gameState == GameState.Running)
			{
				GUILayout.Label("Current player ID=" + currentPlayerID.ToString());
				GUILayout.Label("Your player ID=" + PhotonNetwork.player.ID.ToString());
				
				if (currentPlayerID == PhotonNetwork.player.ID)
				{
					GUILayout.Label("It is your turn!");
				}	
			}
			
			GUILayout.Label(winString);
		}
	}
}