using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This script controls how the bricks are placed.
 * Add to SceneLogic object.
 *
 * A TowerPiece-script needs to be added to new bricks
 **/

namespace TowerVR
{

	public class PlacingBricks : TowerVR.TowerGameBehaviour  {

		public GameObject placingPlane;
		
		// We need a basic case for these in case they have not been set in the Unity Editor
		public List<GameObject> easyBricks = new List<GameObject>();
        public List<GameObject> mediumBricks = new List<GameObject>();
        public List<GameObject> hardBricks = new List<GameObject>();
		
		private List<GameObject> displayedObjects = new List<GameObject>();

		private int turnState;
		private int towerState;
		private int currentPlayerID;

		public GameObject newPiece;
		public GameObject pieceToAdd;
		private Camera myCamera;
		private bool noCube;
		private bool hasPlaced;
		private bool hasSelected;
		private bool selectionPiecesAreSpawned;
		private Bounds objectBounds;
		private Vector3 objectExtent;
		
		// temporary
		private bool selecting = false;

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
			hasPlaced = false;
			hasSelected = false;
			selectionPiecesAreSpawned = false;
		}

		void Update () {
			
			

			//Check if it is my turn, otherwise just observe
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
				// test
				// if (!selecting)
				// {
				// 	Debug.Log("Calling");
				// 	selecting = true;
				// 	GetAndDisplay();
				// }
				
				// if (selecting)
				// {
				// 	// Continuous update not needed?
					
				// 	// ... selecting = false
				// }
				
				
			
				//If it is my turn, spawn new piece to be placed.
				if(turnState == TurnState.SelectingTowerPiece && !hasSelected)
				{
					if (!selectionPiecesAreSpawned)
					{
						Debug.Log("Spawning pieces for selection");
						GetAndDisplay();
						selectionPiecesAreSpawned = true;
					}
					
					if (!hasSelected)
					{
					
						// Selection logic
						RaycastHit hit;
						int piecesToSelectMask = 1 << 9; //Sets Layer 9 to true
						if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 700, piecesToSelectMask))
						{
							// Debug.Log("Hit object " + hit.collider + " wtih RB " + hit.rigidbody + " with object " + hit.transform.gameObject + " at " + hit.distance);
							
							// hit.transform.localScale = new Vector3 (3.0f, 3.0f, 3.0f);
							// hit.transform.localScale = hit.transform.localScale * 2.0f;
							// float xScale = hit.transform.localScale.x;
							// float yScale = hit.transform.localScale.y;
							// float zScale = hit.transform.localScale.z;
							
							// hit.transform.localScale = new Vector3(xScale * 2.0f, yScale * 2.0f, zScale * 2.0f);
							hit.transform.RotateAround(hit.transform.position, hit.transform.up, 2.0f);
							
							if (Cardboard.SDK.Triggered)
							{
								Debug.Log("Selected piece " + hit.transform.gameObject);
								newPiece = hit.transform.gameObject;
								
								for (int i = 0; i < displayedObjects.Count; i++) 
								{
									if (displayedObjects[i] == hit.transform.gameObject)
									{
										switch (i)
										{
											case 0:
												Debug.Log("Difficulty Easy");
												manager.selectTowerPiece(TowerPieceDifficulty.Easy);
												break;
											case 1:
												Debug.Log("Difficulty Medium");
												manager.selectTowerPiece(TowerPieceDifficulty.Medium);
												break;
											case 2:
												Debug.Log("Difficulty Hard");
												manager.selectTowerPiece(TowerPieceDifficulty.Hard);
												break;
											default:
												Debug.Log("Bad: default case");
												manager.selectTowerPiece(TowerPieceDifficulty.Easy);
												break;
										}	
									}
								}
								
								hasSelected = true;
								hasPlaced = false;	// already set
								
								// Reset piece selected state for the future
								selectionPiecesAreSpawned = false;
							}
						}
					
						else
						{
							for (int i = 0; i < displayedObjects.Count; i++) 
							{
								// displayedObjects[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
								// float xScale = displayedObjects[i].transform.localScale.x;
								// float yScale = displayedObjects[i].transform.localScale.y;
								// float zScale = displayedObjects[i].transform.localScale.z;
								
								// displayedObjects[i].transform.localScale = new Vector3(xScale * 0.5f, yScale * 0.5f, zScale * 0.5f);
								displayedObjects[i].transform.RotateAround(hit.transform.position, hit.transform.up, 0.5f);
							}
						}
					}
				}

				//Proceed when turnState is PlacingTowerPiece
				if (turnState == TurnState.PlacingTowerPiece && !hasPlaced && hasSelected)
				{
					Debug.Log("Placing state");

					//Project the new piece directly where you look
					RaycastHit hitInfo;
					int towerLayerMask = 1 << 8; //Sets Layer 8 to true
					//Cast a box from cameras pow
					if(Physics.BoxCast(myCamera.transform.position, objectExtent, myCamera.transform.forward, out hitInfo, newPiece.transform.rotation, 500, towerLayerMask, QueryTriggerInteraction.UseGlobal))
					{
						//Instantiate a new towerpiece if there is none to be placed
						if(noCube)
						{
							Debug.Log("NewPiece: " + newPiece.transform.name);
							pieceToAdd = PhotonNetwork.Instantiate(newPiece.transform.name, newPiece.transform.position, newPiece.transform.rotation, 0) as GameObject;
							noCube = false;
							pieceToAdd.layer = 0;

							Mesh mesh = pieceToAdd.GetComponent<MeshFilter>().mesh;
							objectBounds = mesh.bounds;
							objectExtent = Vector3.Scale(objectBounds.extents, pieceToAdd.transform.localScale); //Get correct bounding box
							//Debug.Log("ObjectExtent: "+ objectExtent);
							
						}
						pieceToAdd.GetComponent<MeshRenderer>().enabled = true;
						pieceToAdd.transform.position = myCamera.transform.position + myCamera.transform.forward * hitInfo.distance + Vector3.up;

						pieceToAdd.transform.rotation = Quaternion.Euler(new Vector3(0, myCamera.transform.rotation.eulerAngles.y, 0));

						//Satisfied? Then place the piece with the button
						if (Cardboard.SDK.Triggered)
						{
							Debug.Log("Button triggered! Placing piece!");
							placeBrick();
						}

					}
					else
					{
						//If there's no intersection with Tower at all, don't render the new piece
						pieceToAdd.GetComponent<MeshRenderer>().enabled = false;
					}
				}
				//Did the time run out? Place it!
				if (turnState == TurnState.TowerReacting && !hasPlaced)
				{
					Debug.Log("Time's up! Placing piece!");
					placeBrick();
				}
			}
			else //!myTurn
			{
				//Observe! Throw things on each other!?
				// Debug.Log("Not my turn");
			}
		}
		
		void placeBrick()
		{
			var rb = pieceToAdd.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.detectCollisions = true;
			rb.useGravity = true;

			pieceToAdd.layer = 8;
			noCube = true;
			hasPlaced = true;
			hasSelected = false;
			manager.placeTowerPiece (pieceToAdd.transform.position.x, pieceToAdd.transform.position.z, pieceToAdd.transform.rotation.y);		
			
			//var photonView = pieceToAdd.GetComponent<PhotonView>();
			//if (photonView)
			//{
			//	photonView.TransferOwnership(PhotonNetwork.masterClient.ID);
			//}
		}
		
		/**
		 *	Randomly select bricks to be displayed for player to choose 
		 **/
		public void GetAndDisplay () 
        {
			// Variables to keep track
            int easyIdx = 0;
            int mediumIdx = 1;
            int hardIdx = 2;
            
            // Debug.Log("set arrays");
			List<GameObject> tempList = new List<GameObject>();
            
            // int randomIdx = Random.Range(0, (easyBricks.Count - 1)); 	// debug
            // Debug.Log("choosing easyBricks[" + randomIdx + "]");
            tempList.Insert(easyIdx, easyBricks[ Random.Range(0, (easyBricks.Count - 1)) ]);
            tempList.Insert(mediumIdx, mediumBricks[ Random.Range(0, (mediumBricks.Count - 1)) ]);
            tempList.Insert(hardIdx, hardBricks[ Random.Range(0, (hardBricks.Count - 1)) ]);
            
            // Debug.Log("Array length: " + tempList.Count);            
            
            /// Set origin positions of objects to display equal to camera positions
            for (int i = 0; i < tempList.Count; i++)
            {
				GameObject temp = Instantiate(tempList[i], new Vector3(), Quaternion.identity) as GameObject;
				temp.name = tempList[i].name;
				var rb = temp.GetComponent<Rigidbody>();
				if (rb != null)
				{
					rb.detectCollisions = true;
				}
				
				displayedObjects.Add(temp);
				
            }

			for (int i = 0; i < displayedObjects.Count; i++) {
				displayedObjects[i].transform.position = new Vector3(myCamera.transform.position.x/1.2f, 3.0f, myCamera.transform.position.z/1.2f);
				displayedObjects[i].transform.LookAt(myCamera.transform);
			}
			
			Vector3 easyObjectWidth = displayedObjects[easyIdx].GetComponent<MeshRenderer>().bounds.size;
			Vector3 mediumObjectWidth = displayedObjects[mediumIdx].GetComponent<MeshRenderer>().bounds.size;			
			Vector3 hardObjectWidth = displayedObjects[hardIdx].GetComponent<MeshRenderer>().bounds.size;
			
			float transDistLeft = mediumObjectWidth.x/2.0f + 1.0f + easyObjectWidth.x/2.0f; 
			float transDistRight = - (mediumObjectWidth.x/2.0f + 1.0f + hardObjectWidth.x/2.0f);
			
			// Debug.Log("distances are " + transDistLeft + ", " + transDistRight);
			
			// Tranform relative to camera's local coordinates
			displayedObjects[easyIdx].transform.Translate(transDistLeft, 0, 0, myCamera.transform);
			displayedObjects[hardIdx].transform.Translate(transDistRight, 0, 0, myCamera.transform);
			
            // Debug.Log("done with generate");
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
