using UnityEngine;
using System.Collections;

/**
 * This script controls how the bricks are placed.
 * Add to SceneLogic object.
 * 
 **/

namespace TowerVR
{

	public class PlacingBricks : TowerVR.TowerGameBehaviour  {

		private int turnState;
		private int towerState;
		private int currentPlayerID;

		public GameObject newPiece;

		public GameObject pieceToAdd;
		private Camera myCamera;
		private bool noCube;
		private bool hasPlaced;
		

		void onTurnStateChanged(int turnState)
		{
			this.turnState = turnState;
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
			manager.turnStateChangedHandlers.Add(onTurnStateChanged);
			manager.towerStateChangedHandlers.Add (onTowerStateChanged);
			manager.nextPlayerTurnHandlers.Add(onNextPlayerTurn);

			myCamera = Camera.main;	
			noCube = true;
					
		}


		void Update () {

			//Check if it is my turn, otherwise just observe
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
				//If it is my turn, spawn new piece to be placed. 
				if(turnState == TurnState.SelectingTowerPiece)
				{
					//newPiece = ngt!
					hasPlaced = false;
					manager.selectTowerPiece(TowerPieceDifficulty.Easy);
				}
								
				
				//Proceed when turnState is PlacingTowerPiece
				if (turnState == TurnState.PlacingTowerPiece && !hasPlaced) 
				{

					//Project the new piece directly where you look
					RaycastHit hitInfo;
					int towerLayerMask = 1 << 8; //Sets Layer 8 to true
					if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hitInfo, Mathf.Infinity, towerLayerMask, QueryTriggerInteraction.UseGlobal))
					{
						//Instantiate a new towerpiece if there is none to be placed	
						if(noCube)
						{
							Debug.Log("NewPiece!");
							pieceToAdd = PhotonNetwork.Instantiate(newPiece.transform.name, newPiece.transform.position, newPiece.transform.rotation, 0) as GameObject;
							pieceToAdd.layer = 0;
							pieceToAdd.GetComponent<Rigidbody>().isKinematic = false;
							//pieceToAdd.GetComponent<Rigidbody>().detectCollisions = false;
							noCube = false;
						}
						pieceToAdd.transform.position = hitInfo.point + Vector3.up * newPiece.transform.localScale.y/2;
						pieceToAdd.transform.rotation = Quaternion.Euler(new Vector3(0, myCamera.transform.rotation.eulerAngles.y, 0));
												
						pieceToAdd.GetComponent<MeshRenderer>().enabled = true; 
						
						//Satisfied? Then place the piece with the button
						if (Cardboard.SDK.Triggered) 
						{
							
							pieceToAdd.GetComponent<Rigidbody>().isKinematic = true;
							//pieceToAdd.GetComponent<Rigidbody>().detectCollisions = true;
							pieceToAdd.layer = 8;
							pieceToAdd.tag = "newTowerPiece";
							noCube = true;
							hasPlaced = true;
							manager.placeTowerPiece (pieceToAdd.transform.position.x, pieceToAdd.transform.position.z, pieceToAdd.transform.rotation.y);
						}
						
					}
					else
					{
						//If there's no intersection with Tower, don't render the new piece
						pieceToAdd.GetComponent<MeshRenderer>().enabled = false;
					}	
				}
			}
			else //!myTurn
			{
				//Observe! Throw things on each other!?
			}
		}
			


		// Destroy listeners
		void OnDestroy()
		{
			manager.turnStateChangedHandlers.Remove(onTurnStateChanged);
			manager.towerStateChangedHandlers.Remove (onTowerStateChanged);
			manager.nextPlayerTurnHandlers.Remove(onNextPlayerTurn);
		}

	}
}
