﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * Handles how the towerpieces are selected and placed.
 * Add to SceneLogic object.
 *
 * A TowerPiece-script needs to be added to new bricks. Also a photonview that syncs transform and the TowerPiece-script
 **/

namespace TowerVR
{

	public class PlacingBricks : TowerVR.TowerGameBehaviour  {
		
		// Arrays of tower piece resources
		public List<GameObject> easyBricks;
        public List<GameObject> mediumBricks;
        public List<GameObject> hardBricks;
		
		// Text objects easy/medium/hard
		public GameObject easyTextObject;
		public GameObject mediumTextObject;
		public GameObject hardTextObject;
		private List<GameObject> textList = new List<GameObject>();
				
		public GameObject pieceToAdd;
		
		private List<GameObject> displayedObjects = new List<GameObject>();

		private int turnState;
		private int towerState;
		private int currentPlayerID;

		private Transform boxTrans;
		private string newPieceName;
		private Camera myCamera;
		private bool noCube;
        private bool materialsSpawned;
        private bool hasPlaced;
		private bool hasSelected;
		private bool selectionPiecesAreSpawned;
		private Bounds objectBounds;
		private Vector3 objectExtent;
		public static Material newMat;
		
		private Material EasyMat;
		private Material MediumMat;
		private Material HardMat;
		
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
            materialsSpawned = false;
            hasPlaced = false;
			hasSelected = false;
			newPieceName = "";
			selectionPiecesAreSpawned = false;
			boxTrans = this.transform;

		}

		void Update () {
			
			
			//Check if it is my turn, otherwise just observe
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
                if(!materialsSpawned)
                {
                    EasyMat = Resources.Load("BrickMaterials/" + SpawnSelectedLevel.LoadedLevel + "EasyMat", typeof(Material)) as Material;
                    MediumMat = Resources.Load("BrickMaterials/" + SpawnSelectedLevel.LoadedLevel + "MediumMat", typeof(Material)) as Material;
                    HardMat = Resources.Load("BrickMaterials/" + SpawnSelectedLevel.LoadedLevel + "HardMat", typeof(Material)) as Material;
                    Debug.Log("Loading materials");
                    materialsSpawned = true;
                }

			
				//If it is my turn, spawn new piece to be placed.
				if(turnState == TurnState.SelectingTowerPiece && !hasSelected)
				{
					if (hasPlaced) //Needs to be reset if time has run out
					{
						hasPlaced = false;
					}
					
					if (!selectionPiecesAreSpawned)
					{
						Debug.Log("Spawning pieces for selection");
						GetAndDisplaySelectionPieces();
						selectionPiecesAreSpawned = true;
					}
					
					
					// Make sure the displayed objects have collision detection on
					// This is a very ineffective place to do it but it doesn't seem to work in GetAndDisplaySelectionPieces
					for (int i = 0; i < displayedObjects.Count; i++) 
					{				
						var rb =  displayedObjects[i].GetComponent<Rigidbody>();
						if(rb != null)
						{
							rb.detectCollisions = true;	
							rb.isKinematic = true;	
							// Debug.Log("Rigidbody for " + rb + ": kinematic = " + rb.isKinematic + ", detectCol = " + rb.detectCollisions + ", w/ layer " + displayedObjects[i].layer);
						}
						
						var col = displayedObjects[i].GetComponent<MeshCollider>();
						//col.convex = true;
						col.isTrigger = true;
					}
					
					for (int i = 0; i < textList.Count; i++) 
					{	
						// textList[i].transform.RotateAround(textList[i].transform.position, textList[i].transform.up, 0.3f);
						textList[i].transform.LookAt(myCamera.transform);
						textList[i].transform.Rotate(0, 180, 0);		
					}
					
					// Selection logic
					RaycastHit hit;
					int piecesToSelectMask = 1 << 9; //Sets Layer 9 to true
					if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 700, piecesToSelectMask))
					{
						// hit.transform.GetComponent<SelectionPieceHovering>().HoveringBehaviour();
						hit.transform.RotateAround(hit.transform.position, myCamera.transform.up, 2.0f);
						
						Behaviour halo = (Behaviour)hit.transform.GetComponent("Halo");  
						halo.enabled = true;
						
						if (Cardboard.SDK.Triggered)
						{
							// Debug.Log("Selected piece " + hit.transform.gameObject);
							newPieceName = hit.transform.gameObject.name;
							newMat = hit.transform.gameObject.GetComponent<Renderer>().material;
							
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
							
							//Debug.Log("Time for select: " + Time.time);
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
							
							Behaviour halo = (Behaviour)displayedObjects[i].GetComponent("Halo");  
							halo.enabled = false;
						}
					}
					
				}

				//Proceed when turnState is PlacingTowerPiece
				if (turnState == TurnState.PlacingTowerPiece && !hasPlaced)
				{
					
					// If time ran out and turn proceeded without player choosing piece
					if (!hasSelected)
					{
						Debug.Log("Time's up! Selecting default piece.");
						newPieceName = displayedObjects[0].name;	// get the easy piece
						manager.selectTowerPiece(TowerPieceDifficulty.Easy);
						hasSelected = true;
						ClearSelectionPieces();
					}

					//Project the new piece directly where you look
					RaycastHit hitInfo;
					int towerLayerMask = 1 << 8; //Sets Layer 8 to true
					//Cast a box from cameras pow
					if(Physics.BoxCast(myCamera.transform.position, objectExtent, myCamera.transform.forward, out hitInfo, boxTrans.rotation, 500, towerLayerMask, QueryTriggerInteraction.UseGlobal))
					{
						//Instantiate a new towerpiece if there is none to be placed
						if(noCube)
						{
							//Debug.Log("NewPiece: " + newPieceName + "   Time to place: " + Time.time);							
							pieceToAdd = PhotonNetwork.Instantiate(newPieceName, new Vector3(), boxTrans.rotation, 0) as GameObject;
							//pieceToAdd.GetComponent<Renderer>().material = newMat;
							// Behaviour halo = (Behaviour)pieceToAdd.GetComponent("Halo");
							// halo.enabled = false;
							
							noCube = false;
							pieceToAdd.layer = 0;

							Mesh mesh = pieceToAdd.GetComponent<MeshFilter>().mesh;
							objectBounds = mesh.bounds;
							objectExtent = Vector3.Scale(objectBounds.extents, pieceToAdd.transform.localScale); //Get correct bounding box
							// Debug.Log("ObjectExtent: "+ objectExtent);
							
						}
						pieceToAdd.GetComponent<MeshRenderer>().enabled = true;
						pieceToAdd.transform.position = myCamera.transform.position + myCamera.transform.forward * hitInfo.distance + Vector3.up;
						pieceToAdd.transform.rotation = Quaternion.Euler(new Vector3(-90, myCamera.transform.rotation.eulerAngles.y, 0));	// All the current pieces are rotated 90 degrees	
						boxTrans = pieceToAdd.transform;
							
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
						//Must check noCube or else we are gonna delete the old cube in some cases
						if (pieceToAdd != null && noCube == false)
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
		
		/**
		*	Places towerpiece at current position
		**/
		void placeBrick()
		{			
			var col = pieceToAdd.GetComponent<MeshCollider>();
			col.isTrigger = false;
			//col.convex = false;

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
            
			List<GameObject> tempList = new List<GameObject>();
            
            tempList.Insert(easyIdx, easyBricks[ Random.Range(0, (easyBricks.Count)) ]);
            tempList.Insert(mediumIdx, mediumBricks[ Random.Range(0, (mediumBricks.Count)) ]);
            tempList.Insert(hardIdx, hardBricks[ Random.Range(0, (hardBricks.Count)) ]);
			
            /// Set origin positions of objects to display equal to camera positions
            for (int i = 0; i < tempList.Count; i++)
            {
				GameObject temp = Instantiate(tempList[i], new Vector3(), Quaternion.identity) as GameObject;
				temp.name = tempList[i].name;
				
				// Turn off halo initially (because we won't remember/know to keep it disabled in the editor)
				// Behaviour halo = (Behaviour)temp.GetComponent("Halo");
				// halo.enabled = false;
				
				//temp.layer = 9;
				displayedObjects.Add(temp);
            }

			for (int i = 0; i < displayedObjects.Count; i++)
			{
				displayedObjects[i].transform.position = myCamera.transform.position/1.4f;
				displayedObjects[i].transform.LookAt(myCamera.transform);
				displayedObjects[i].transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
				
				// Scale object to half size
				// Vector3 currentSize = displayedObjects[i].GetComponent<MeshFilter>().mesh.bounds.size;
				Vector3 scale = displayedObjects[i].transform.localScale;
				scale.x = 0.3f * scale.x;
				scale.y = 0.3f * scale.y;
				scale.z = 0.3f * scale.z;
				displayedObjects[i].transform.localScale = scale;
 
				displayedObjects[i].layer = 9;
				
				
			}
			
			displayedObjects[easyIdx].GetComponent<Renderer>().material = EasyMat;
			displayedObjects[mediumIdx].GetComponent<Renderer>().material = MediumMat;
			displayedObjects[hardIdx].GetComponent<Renderer>().material = HardMat;
			
			Vector3 easyObjectWidth = Vector3.Scale(displayedObjects[easyIdx].GetComponent<MeshFilter>().mesh.bounds.extents, displayedObjects[easyIdx].transform.localScale);
			Vector3 mediumObjectWidth = Vector3.Scale(displayedObjects[mediumIdx].GetComponent<MeshFilter>().mesh.bounds.extents, displayedObjects[mediumIdx].transform.localScale);			
			Vector3 hardObjectWidth = Vector3.Scale(displayedObjects[hardIdx].GetComponent<MeshFilter>().mesh.bounds.extents, displayedObjects[hardIdx].transform.localScale);
			
			// TODO: check validity of signs
			// Vector3 transDistLeft = myCamera.transform.right * (mediumObjectWidth.x/4.0f + 1.0f + easyObjectWidth.x/4.0f); 
			// Vector3 transDistRight = myCamera.transform.right * -(mediumObjectWidth.x/4.0f + 1.0f + hardObjectWidth.x/4.0f);
			float transDistLeft = -(mediumObjectWidth.x + 5.0f + easyObjectWidth.x); 
			float transDistRight = mediumObjectWidth.x + 5.0f + hardObjectWidth.x;
			
			// Debug.Log("distances: " + transDistLeft + ", " + transDistRight);
			
			// Tranform relative to camera's local coordinates
			displayedObjects[easyIdx].transform.Translate(transDistLeft, 0, 0, myCamera.transform);
			displayedObjects[mediumIdx].transform.Translate(0, 0, 0, myCamera.transform);
			displayedObjects[hardIdx].transform.Translate(transDistRight, 0, 0, myCamera.transform);
			
			// Add text
			GameObject easyText = Instantiate(easyTextObject, new Vector3(), Quaternion.identity) as GameObject;
			GameObject mediumText = Instantiate(mediumTextObject, new Vector3(), Quaternion.identity) as GameObject;
			GameObject hardText = Instantiate(hardTextObject, new Vector3(), Quaternion.identity) as GameObject;
			textList.Add(easyText);
			textList.Add(mediumText);
			textList.Add(hardText);
			
			textList[0].transform.position = new Vector3(displayedObjects[easyIdx].transform.position.x, displayedObjects[easyIdx].transform.position.y + 8.0f, displayedObjects[easyIdx].transform.position.z);
			textList[1].transform.position = new Vector3(displayedObjects[mediumIdx].transform.position.x, displayedObjects[mediumIdx].transform.position.y + 8.0f, displayedObjects[mediumIdx].transform.position.z);
			textList[2].transform.position = new Vector3(displayedObjects[hardIdx].transform.position.x, displayedObjects[hardIdx].transform.position.y + 8.0f, displayedObjects[hardIdx].transform.position.z);
        }
		
		void ClearSelectionPieces()
		{
			Debug.Log("Clearing selection pieces");
			
			for (int i = 0; i < displayedObjects.Count; i++)
			{
				Destroy(displayedObjects[i]);	
			}
			
			for (int i = 0; i < textList.Count; i++)
			{
				Destroy(textList[i]);	
			}
			displayedObjects.Clear();
			textList.Clear();
			
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
