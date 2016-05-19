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
		
		// We need a basic case for these in case they have not been set in the Unity Editor
		public List<GameObject> easyBricks;
        public List<GameObject> mediumBricks;
        public List<GameObject> hardBricks;
		
		private List<GameObject> displayedObjects = new List<GameObject>();

		private int turnState;
		private int towerState;
		private int currentPlayerID;

		public GameObject newPiece;
		private string newPieceName;
		public GameObject pieceToAdd;
		private Camera myCamera;
		private bool noCube;
		private bool hasPlaced;
		private bool hasSelected;
		private bool selectionPiecesAreSpawned;
		private Bounds objectBounds;
		private Vector3 objectExtent;
		
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
			newPieceName = "";
			selectionPiecesAreSpawned = false;
		}

		void Update () {
			
			

			//Check if it is my turn, otherwise just observe
			if (currentPlayerID == PhotonNetwork.player.ID)
			{	
			
				//If it is my turn, spawn new piece to be placed.
				if(turnState == TurnState.SelectingTowerPiece && !hasSelected)
				{
					if (hasPlaced) 
						hasPlaced = false;
					
					if (!selectionPiecesAreSpawned)
					{
						Debug.Log("Spawning pieces for selection");
						GetAndDisplaySelectionPieces();
						selectionPiecesAreSpawned = true;
					}
					
					if (!hasSelected)
					{
						
						// Make sure the displayed objects have collision detection on
						// This is a very ineffective place to do it but it doesn't seem to work in GetAndDisplaySelectionPieces
						for (int i = 0; i < displayedObjects.Count; i++) 
						{
							var rb =  displayedObjects[i].GetComponent<Rigidbody>();
							rb.detectCollisions = true;	
							rb.isKinematic = true;			
					 		// Debug.Log("Rigidbody for " + rb + ": kinematic = " + rb.isKinematic + ", detectCol = " + rb.detectCollisions + ", w/ layer " + displayedObjects[i].layer);
						}
						
						// Selection logic
						RaycastHit hit;
						int piecesToSelectMask = 1 << 9; //Sets Layer 9 to true
						if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 700, piecesToSelectMask))
						{
							// Debug.Log("Hit object " + hit.collider + " wtih RB " + hit.rigidbody + " with object " + hit.transform.gameObject + " at " + hit.distance);
							// hit.transform.GetComponent<SelectionPieceHovering>().HoveringBehaviour();
							hit.transform.RotateAround(hit.transform.position, hit.transform.up, 2.0f);
							// // // Behaviour halo = (Behaviour)hit.transform.GetComponent("Halo");  
							// halo.enabled = true;
							
							if (Cardboard.SDK.Triggered)
							{
								// Debug.Log("Selected piece " + hit.transform.gameObject);
								newPieceName = hit.transform.gameObject.name;
								
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
								Debug.Log("Time for select: " + Time.time);
								hasSelected = true;
								
								// Clear Selection pieces
								 ClearSelectionPieces();
							}
						}
					
						else
						{
							for (int i = 0; i < displayedObjects.Count; i++) 
							{
								// displayedObjects[i].GetComponent<SelectionPieceHovering>().ConstantBehaviour();
								displayedObjects[i].transform.RotateAround(displayedObjects[i].transform.position, displayedObjects[i].transform.up, 0.3f);
								// // Behaviour halo = (Behaviour)displayedObjects[i].GetComponent("Halo");  
								// halo.enabled = false;
							}
						}
					}
				}

				//Proceed when turnState is PlacingTowerPiece
				if (turnState == TurnState.PlacingTowerPiece && !hasPlaced)
				{
					
					// If time ran out and turn proceeded without player choosing piece
					if (!hasSelected)
					{
						Debug.Log("PLACING STATE WITHOUT SELECTION");
						newPieceName = displayedObjects[0].name;	// get the easy piece
						manager.selectTowerPiece(TowerPieceDifficulty.Easy);
						hasSelected = true;
						ClearSelectionPieces();
					}
					
					//Project the new piece directly where you look
					RaycastHit hitInfo;
					int towerLayerMask = 1 << 8; //Sets Layer 8 to true
					//Cast a box from cameras pow
					if(Physics.BoxCast(myCamera.transform.position, objectExtent, myCamera.transform.forward, out hitInfo, newPiece.transform.rotation, 500, towerLayerMask, QueryTriggerInteraction.UseGlobal))
					{
						//Instantiate a new towerpiece if there is none to be placed
						if(noCube)
						{
							// Debug.Log("NewPiece: " + newPiece.transform.name);
							Debug.Log("NewPiece: " + newPieceName + "   Time to place: " + Time.time);							
							// pieceToAdd = PhotonNetwork.Instantiate(newPiece.transform.name, newPiece.transform.position, newPiece.transform.rotation, 0) as GameObject;
							pieceToAdd = PhotonNetwork.Instantiate(newPieceName, new Vector3(), Quaternion.identity, 0) as GameObject;
							
							// Behaviour halo = (Behaviour)pieceToAdd.GetComponent("Halo");
							// halo.enabled = false;
							
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
						if (pieceToAdd != null)
						{
							pieceToAdd.GetComponent<MeshRenderer>().enabled = false;
						}
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
			pieceToAdd.tag = "newTowerPiece";
			noCube = true;
			hasPlaced = true;
			hasSelected = false;
			manager.placeTowerPiece(pieceToAdd.transform.position.x, pieceToAdd.transform.position.z, pieceToAdd.transform.rotation.y);		
			
			selectionPiecesAreSpawned = false;

		}
		
		/**
		 *	Randomly select bricks to be displayed for player to choose 
		 **/
		void GetAndDisplaySelectionPieces () 
        {
			// Variables to keep track
            int easyIdx = 0;
            int mediumIdx = 1;
            int hardIdx = 2;
            
            // Debug.Log("set arrays");
			List<GameObject> tempList = new List<GameObject>();
            
            tempList.Insert(easyIdx, easyBricks[ Random.Range(0, (easyBricks.Count)) ]);
            tempList.Insert(mediumIdx, mediumBricks[ Random.Range(0, (mediumBricks.Count)) ]);
            tempList.Insert(hardIdx, hardBricks[ Random.Range(0, (hardBricks.Count)) ]);
			
            /// Set origin positions of objects to display equal to camera positions
            for (int i = 0; i < tempList.Count; i++)
            {
				GameObject temp = Instantiate(tempList[i], new Vector3(), Quaternion.identity) as GameObject;
				temp.name = tempList[i].name;
				
				// // Turn off halo initially (because we won't remember/know to keep it disabled in the editor)
				// // Behaviour halo = (Behaviour)temp.GetComponent("Halo");
				// halo.enabled = false;
				
				temp.layer = 9;
				displayedObjects.Add(temp);
            }

			for (int i = 0; i < displayedObjects.Count; i++) {
				displayedObjects[i].transform.position = new Vector3(myCamera.transform.position.x/2.0f, 6.0f, myCamera.transform.position.z/2.0f);
				displayedObjects[i].transform.LookAt(myCamera.transform);
				var rb = displayedObjects[i].GetComponent<Rigidbody>();
				if (rb != null)
				{
					// Debug.Log("coming in????");
					rb.detectCollisions = true;
					rb.isKinematic = true;
				}
				displayedObjects[i].layer = 9;
			}
			
			Vector3 easyObjectWidth = Vector3.Scale(displayedObjects[easyIdx].GetComponent<MeshRenderer>().bounds.extents, displayedObjects[easyIdx].transform.localScale);
			Vector3 mediumObjectWidth = Vector3.Scale(displayedObjects[mediumIdx].GetComponent<MeshRenderer>().bounds.extents, displayedObjects[mediumIdx].transform.localScale);			
			Vector3 hardObjectWidth = Vector3.Scale(displayedObjects[hardIdx].GetComponent<MeshRenderer>().bounds.extents, displayedObjects[hardIdx].transform.localScale);
			
			// Todo: check validity of signs
			float transDistLeft = mediumObjectWidth.x/4.0f + 1.0f + easyObjectWidth.x/4.0f; 
			float transDistRight = - (mediumObjectWidth.x/4.0f + 1.0f + hardObjectWidth.x/4.0f);
			
			// Debug.Log("distances are " + transDistLeft + ", " + transDistRight);
			
			// Tranform relative to camera's local coordinates
			displayedObjects[easyIdx].transform.Translate(transDistLeft, 0, 0, myCamera.transform);
			displayedObjects[hardIdx].transform.Translate(transDistRight, 0, 0, myCamera.transform);
			
            // Debug.Log("done with generate");
        }
		
		void ClearSelectionPieces()
		{
			Debug.Log("Clearing selection pieces");
			
			for (int i = 0; i < displayedObjects.Count; i++)
			{
				Destroy(displayedObjects[i]);	
			}
			
			displayedObjects = new List<GameObject>();
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
