using UnityEngine;
using System.Collections;

/**
 * This script controls how the bricks are placed.
 * Add to SceneLogic object.
 *
 * A TowerPiece-script needs to be added to new bricks
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
		private bool hasSelected;
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

		}


		void Update () {

			//Check if it is my turn, otherwise just observe
			if (currentPlayerID == PhotonNetwork.player.ID)
			{
				//If it is my turn, spawn new piece to be placed.
				if(turnState == TurnState.SelectingTowerPiece && !hasSelected)
				{
					//newPiece = ngt!
					Debug.Log("Selecting piece");

					manager.selectTowerPiece(TowerPieceDifficulty.Easy);
					hasSelected = true;
					hasPlaced = false;
				}


				//Proceed when turnState is PlacingTowerPiece
				if (turnState == TurnState.PlacingTowerPiece && !hasPlaced)
				{

					//Project the new piece directly where you look
					RaycastHit hitInfo;
					int towerLayerMask = 1 << 8; //Sets Layer 8 to true
					//Cast a box from cameras pow
					if(Physics.BoxCast(myCamera.transform.position, objectExtent, myCamera.transform.forward, out hitInfo, newPiece.transform.rotation, 500, towerLayerMask, QueryTriggerInteraction.UseGlobal))
					{
						//Instantiate a new towerpiece if there is none to be placed
						if(noCube)
						{
							Debug.Log("NewPiece!");
							pieceToAdd = PhotonNetwork.Instantiate(newPiece.transform.name, newPiece.transform.position, newPiece.transform.rotation, 0) as GameObject;
							noCube = false;
							pieceToAdd.layer = 0;

							Mesh mesh = pieceToAdd.GetComponent<MeshFilter>().mesh;
							objectBounds = mesh.bounds;
							objectExtent = Vector3.Scale(objectBounds.extents, pieceToAdd.transform.localScale); //Get correct bounding box
						}
						//if normal is up
						if(hitInfo.normal == Vector3.up)
						{
							pieceToAdd.transform.position = myCamera.transform.position + myCamera.transform.forward * hitInfo.distance + Vector3.up;
							pieceToAdd.GetComponent<MeshRenderer>().enabled = true;
						}
						else
						{
							RaycastHit newHitInfo;
							if(Physics.BoxCast(hitInfo.point, objectExtent, Vector3.down, out newHitInfo, newPiece.transform.rotation, 500, towerLayerMask, QueryTriggerInteraction.UseGlobal))
							{
								pieceToAdd.transform.position = (myCamera.transform.position + myCamera.transform.forward * hitInfo.distance)  + Vector3.down * newHitInfo.distance + Vector3.up;
								pieceToAdd.GetComponent<MeshRenderer>().enabled = true;
							}
							else
							{
								//If there's no intersection with Tower below, don't render the new piece
								pieceToAdd.GetComponent<MeshRenderer>().enabled = false;
							}

						}

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
