using UnityEngine;
using System.Collections;

/**
* Updates position for a player object by setting its position to each player's client camera position
*/
public class PlayerWispCameraMovement : Photon.MonoBehaviour {
	public bool debugPosition = false;
	private GameObject mainCamera;

	private ParticleSystem[] particleSystems;

	void Awake()
	{
		particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	void Start () {
		if (photonView.isMine)
		{
			Debug.Log("Instantiated own Player object");
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");	//cardboard main camera
			
			foreach (var childTransform in gameObject.GetComponentsInChildren<Transform>())
			{
				if (childTransform != transform)
				{
					childTransform.gameObject.SetActive(false);
				}
			}
		}
		
		else
		{
			Debug.Log("Instantiated other Player object");
			foreach (var system in particleSystems)
			{
				// Should render particles for other players
				system.Play();
			}
		}
	}

	/**
	* Update position of player object by setting it to camera position
	*/
	void Update () 
	{
		// Make player object invisible and update its position for all players in the network room
		if (photonView.isMine) {
			transform.position = mainCamera.transform.position;
			transform.rotation = mainCamera.transform.rotation;
			
			if (debugPosition)
			{
				Debug.Log("Pos. of my player obj. set to: " + mainCamera.transform.position);
			}
		} 
	}
}
