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
		
		private GameObject replayButton;
		private GameObject mainMenuButton;

		private int gameState = GameState.AwaitingPlayers;

		void onGameStateChanged(int gameState)
		{
			if (gameState == GameState.Ended)
			{
				handleGameEnded();
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
		
		void Start()
		{
			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			if (DEBUG_SPAWN)
			{
				handleGameEnded();
			}
		}
		
		void Update()
		{
			replayButton.transform.RotateAround(replayButton.transform.position, Vector3.up, 20 * Time.deltaTime);
			mainMenuButton.transform.RotateAround(mainMenuButton.transform.position, Vector3.up, 20 * Time.deltaTime);
		}
		
		void OnDestroy()
		{
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
		}
	}	
}