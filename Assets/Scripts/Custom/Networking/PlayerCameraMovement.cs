using UnityEngine;
using System.Collections;

/**
* Updates position for a player object by setting its position to each player's client camera position
*/
public class PlayerCameraMovement : Photon.MonoBehaviour {
	
	public bool debugPosition = false;
	private MeshRenderer meshRenderer;
	private GameObject thisCamera;
	private PhotonView thisPhotonView;

	void Start () {
		thisCamera = GameObject.FindGameObjectWithTag("MainCamera");	//cardboard main camera
		thisPhotonView = this.GetComponent<PhotonView>();
		meshRenderer = this.GetComponent<MeshRenderer>();
	}

	/**
	* Update position of player object by setting it to camera position
	*/
	void Update () {
		
		// Make player object invisible and update its position for all players in the network room
		if (thisPhotonView.isMine) {
			meshRenderer.enabled = false;

			transform.position = thisCamera.transform.position;
			transform.rotation = thisCamera.transform.rotation;
			
			if (debugPosition)
			{
				Debug.Log("Pos. of my player obj. set to: " + thisCamera.transform.position);
			}
		} 
		
		// Ensure player objects of other players in the network room are visible
		else {
			meshRenderer.enabled = true;
		}
	}
}
