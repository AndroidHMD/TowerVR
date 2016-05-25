using UnityEngine;
using System.Collections;

namespace TowerVR
{
	public class HandleEndGame : TowerGameBehaviour 
	{
		public Vector3 replayButtonPosition;
		public Vector3 mainMenuButtonPosition;
		
		public GameObject replayButtonPrefab;
		public GameObject mainMenuButtonPrefab;
		
		public Collider heightSensor;
		
		public bool DEBUG_SPAWN = false;
		
		public GameObject winnerTrophy;
		public GameObject loserTrophy;
		
		public float trophyCameraDistance = 50.0f;
		
		private GameObject replayButton;
		private GameObject mainMenuButton;
		private GameObject trophy;

		private int gameState = GameState.AwaitingPlayers;

		void onGameStateChanged(int gameState)
		{
			if (gameState == GameState.Ended)
			{
				handleGameEnded();
			}
		}
		
		void onPlayerWon(int playerID)
		{
			if (playerID == PhotonNetwork.player.ID)
			{
				spawnTrophy(winnerTrophy);
			}
			
			else
			{
				spawnTrophy(loserTrophy);
			}
		}
		
		void handleGameEnded()
		{
			Destroy(heightSensor);
			
			replayButton = Instantiate(replayButtonPrefab) as GameObject;
			replayButton.transform.position = replayButtonPosition;
			
			mainMenuButton = Instantiate(mainMenuButtonPrefab) as GameObject;
			mainMenuButton.transform.position = mainMenuButtonPosition;
		}
		
		void spawnTrophy(GameObject obj)
		{
			var cameraTransform = Camera.main.transform;
			
			Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * trophyCameraDistance;
			trophy = Instantiate(obj) as GameObject;
			
			trophy.transform.position = spawnPosition;
		}
		
		
		void Start()
		{
			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			manager.playerWonHandlers.Add(onPlayerWon);
			if (DEBUG_SPAWN)
			{
				handleGameEnded();
			}
		}
		
		void Update()
		{
			if (replayButton != null)
			{
				rotateAroundOwnAxis(replayButton);
			}
			
			if (mainMenuButton != null)
			{
				rotateAroundOwnAxis(mainMenuButton);
			}
			
			if (trophy != null)
			{
				rotateAroundOwnAxis(trophy);
			}
		}
		
		void rotateAroundOwnAxis(GameObject obj)
		{
			obj.transform.RotateAround(obj.transform.position, Vector3.up, 20 * Time.deltaTime);
		}
		
		void OnDestroy()
		{
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
			manager.playerWonHandlers.Remove(onPlayerWon);
		}
	}	
}