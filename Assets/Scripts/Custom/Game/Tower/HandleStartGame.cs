﻿using UnityEngine;
using System.Collections;

namespace TowerVR
{
	public class HandleStartGame : TowerVR.TowerGameBehaviour 
	{
		public GameObject startButton;
		public static bool countdownFinished;
		
		private int turnState;
		private int gameState;
		private int currentPlayerID;
		
		private bool noButton = true;
		private bool started = false;
		private bool notStartedCountdown = true;
		private GameObject countdownNumber;
		private GameObject tempButton;
		private Vector3 displayPos;
		private Camera myCamera;
		
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
		
		
		void Start () 
		{
			manager.turnStateChangedHandlers.Add(onTurnStateChanged);
			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Add(onNextPlayerTurn);
			countdownFinished = false;
			displayPos = new Vector3(0, 25, 0);
			myCamera = Camera.main;
			
			Debug.Log("I'm ready yo!");
			manager.notifyIsReady();
		}
		
		void OnDestroy()
		{
			manager.turnStateChangedHandlers.Remove(onTurnStateChanged);
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
			manager.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
		}
		
		
		void Update() 
		{
			
			if (gameState == GameState.AllPlayersReady && !started)
            {
                if (PhotonNetwork.isMasterClient)
				{
					if(noButton)
					{
						tempButton = Instantiate(startButton, displayPos, Quaternion.identity) as GameObject;
						tempButton.layer = 9;
						noButton = false;
					}
					RaycastHit hit;
					int newLayerMask = 1 << 9; 
					if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 100, newLayerMask , QueryTriggerInteraction.UseGlobal))
					{
						//TODO: Change color? Animation
						tempButton.GetComponent<Renderer>().material.color = Color.yellow;
						if(Cardboard.SDK.Triggered)
						{
							Debug.Log("Let's start this game!");
							manager.tryStartGame(); //Starts countdown
							//TODO: animate
							Destroy(tempButton);
							started = true;
						}
					}
					else
					{
						tempButton.GetComponent<Renderer>().material.color = Color.red;
					}

				}
            }
			
			
			if (gameState == GameState.Countdown)
			{
				if (PhotonNetwork.isMasterClient && notStartedCountdown)
				{
					Debug.Log("Start countdown!");
					StartCoroutine(countdown());
					//TODO: Use Photon time sync for syncing the countdown for all clients?
					notStartedCountdown = false;
				}
			
			}
			
			if(gameState == GameState.Ended)
			{
				//TODO: Display that game ended
				Debug.Log("Game has ended!");
			}
			
			if(gameState == GameState.Stopped)
			{
				//TODO: Display some shiet
				Debug.Log("Game has stopped!");
			}
		}
		
		//Debugging
		void OnGUI()
		{
			GUILayout.Label("");
			GUILayout.Label("Player count: " + PhotonNetwork.playerList.Length);
			
			GUILayout.Label("GameState: " + GameState.ToString(gameState));
			GUILayout.Label("TurnState: " + TurnState.ToString(turnState));
			
			if (gameState == GameState.Running)
			{
				GUILayout.Label("Current player ID=" + currentPlayerID.ToString());
				GUILayout.Label("Your player ID=" + PhotonNetwork.player.ID.ToString());
				
				if (currentPlayerID == PhotonNetwork.player.ID)
				{
					GUILayout.Label("It is your turn!");
				}	
			}
		}
		
		private IEnumerator countdown() {
			
			//TODO: Animate number (scale up and rotate)
			int nrCountdowns = 3;
			int countdownTime = 1;
			
			for (int i = 0; i<nrCountdowns; i++)
			{
				string nrName = "Countdown-";
				Debug.Log("Number: " + (nrCountdowns-i));
				nrName = nrName + (nrCountdowns-i);
				countdownNumber = PhotonNetwork.Instantiate(nrName, displayPos, Quaternion.identity, 0) as GameObject;
				
				Debug.Log("Time1: " + Time.time);
				yield return new WaitForSeconds(countdownTime);
				Debug.Log("Time2: " + Time.time);		
					
				PhotonNetwork.Destroy(countdownNumber);
				
			}
							
			countdownFinished = true;
	
		}
	}
}