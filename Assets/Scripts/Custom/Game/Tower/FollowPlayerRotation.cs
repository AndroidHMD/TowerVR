using UnityEngine;
using System.Collections;

/*
 *  The placing plane should follow the transform of player whose turn it it.
 * */

namespace TowerVR
{
	public class FollowPlayerRotation : TowerVR.TowerGameBehaviour  {

		private int gameState;
		private int towerState;
		private int currentPlayerID;

		private MeshRenderer thisMesh;



		void onGameStateChanged(int gameState)
		{
			this.gameState = gameState;
		}

		void onTowerStateChanged(int towerState)
		{
			this.towerState = towerState;
		}

		void onNextPlayerTurn(int nextPlayerID)
		{
			this.currentPlayerID = nextPlayerID;
		}

		void Start () 
		{
			gameState = GameState.AwaitingPlayers;

			manager.gameStateChangedHandlers.Add(onGameStateChanged);
			manager.towerStateChangedHandlers.Add (onTowerStateChanged);
			manager.nextPlayerTurnHandlers.Add(onNextPlayerTurn);

			thisMesh = this.GetComponent<MeshRenderer>();
		}

		// Update is called once per frame
		void Update () {
		
			//Is it my turn?
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
				//Rotation should follow the player
				transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
			}

			// Should we move up?
			if (towerState == TowerState.IncreasingHeight) 
			{
				transform.position += (Vector3.up * 5);
			}

			// Is the tower falling?
			if (towerState == TowerState.Falling)
			{
				//Don't draw the grid
				thisMesh.enabled = false;
			} else {
				thisMesh.enabled = true;
			}

		}

		void OnDestroy()
		{
			manager.gameStateChangedHandlers.Remove(onGameStateChanged);
			manager.towerStateChangedHandlers.Remove (onTowerStateChanged);
			manager.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
		}
	}
}
