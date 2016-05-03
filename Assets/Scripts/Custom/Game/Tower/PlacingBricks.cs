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

		public GameObject placingPlane;

		private GameObject newPiece;
		private Camera myCamera;
		private Vector3 intersectionWithPlane;
		
		//TODO: remove all these variables
		private bool justDoIt = true;


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
					
		}


		void Update () {

			//Check if it is my turn, otherwise just observe
			//if (currentPlayerID == PhotonNetwork.player.ID)
			if(justDoIt)
			{
				//If it is my turn, spawn new piece to be placed. 
				//Change turnState to SelectingTowerPiece by adding function to TowerGameManager and call it!
				//newPiece = manager.selectTowerPiece();
				//Change turnState to PlacingTowerPiece
				
				//Temporary solution
				newPiece = GameObject.FindGameObjectWithTag("TowerPiece");


				//TODO: change back when the rest is implemented
				//Proceed when turnState is PlacingTowerPiece
				if(justDoIt)
				//if (turnState == TurnState.PlacingTowerPiece) 
				{


					//Check where cameraRay intersect with grid
					if (findIntersection (out intersectionWithPlane)) 
					{
						
						
						///The TowerPiece should be projected onto the tower
						//VERSION 1
						RaycastHit hitInfo;
						int towerLayerMask = 1 << 8; //Sets Layer 8 to true
						if(Physics.Raycast(intersectionWithPlane, Vector3.down, out hitInfo, Mathf.Infinity, towerLayerMask, QueryTriggerInteraction.UseGlobal))
						{
							//TODO: Only render outlines?
							newPiece.GetComponent<MeshRenderer>().enabled = true; 
							newPiece.transform.position = hitInfo.point + Vector3.up * newPiece.transform.localScale.y/2;
						}
						else
						{
							//If there's no intersection with Tower, don't render the new piece
							newPiece.GetComponent<MeshRenderer>().enabled = false;
						}
						
						//VERSION 2
						//Translate down so long we don't hit a collider
						//var newCol = newPiece.GetComponent<Collider>();
						//while(!collisions) -> move down
											
						
						//newPiece.transform.position = intersectionWithPlane;	//See the prick on placing plane			
						newPiece.transform.rotation = Quaternion.Euler(new Vector3(0, myCamera.transform.rotation.eulerAngles.y, 0));
						
						//Satisfied? Then place the piece with the button
						if (Cardboard.SDK.Triggered) 
						{
												
							Debug.Log("Placing new piece!");
							GameObject pieceToAdd;
							pieceToAdd = PhotonNetwork.Instantiate(newPiece.transform.name, newPiece.transform.position, newPiece.transform.rotation, 0) as GameObject;
							pieceToAdd.tag = "newTowerPiece";
							manager.placeTowerPiece (newPiece.transform.position.x, newPiece.transform.position.z, newPiece.transform.rotation.y);
						}
						
					}
					 	
				}
				
			}
			else 
			{
				//Observe! Throw things on each other!?

			}
			
		}
			

		/**
		 *	Find intersection with placing plane 
		 **/
		bool findIntersection(out Vector3 intersectionWithPlane)
		{
			//Towerpiece should move in the grid above the playing field
			Vector3 planePos = placingPlane.transform.position;

			// create an infinite plane at placingPlanes position with normal pointing downward
			Plane myPlane = new Plane (Vector3.down, planePos);

			//Have the camera ray point slightly upward
			Ray cameraRay = myCamera.ScreenPointToRay(
				new Vector3(Screen.currentResolution.width/2, (5*Screen.currentResolution.height)/6, 0.0f));
				
				
			// For debugging purposes
			GameObject debugRay = GameObject.FindGameObjectWithTag("RayDebug");				

			//If the ray hits the plane, return point of intersection
			float dist;
			if(myPlane.Raycast(cameraRay, out dist))
			{			
				Debug.Log("Found intersection");
				intersectionWithPlane = cameraRay.GetPoint (dist);
				
				// For debugging purposes
				debugRay.GetComponent<MeshRenderer>().enabled = true;
				debugRay.transform.position = myCamera.transform.position + Vector3.up*2;
				debugRay.transform.forward = intersectionWithPlane - myCamera.transform.position;
				
				return true;
			}
			else
			{
				debugRay.GetComponent<MeshRenderer>().enabled = false;
				intersectionWithPlane = planePos;
				return false;
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
