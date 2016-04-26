using UnityEngine;
using System.Collections;

public class PlayerCameraMovement : MonoBehaviour {
	public Transform cameraTransform;
	public PhotonView photonView;

	public MeshRenderer meshRenderer;

	void Start () {
		cameraTransform = GameObject.FindGameObjectWithTag("CameraAR").transform;
	}

	void Update () {
		if (photonView.isMine) {
			meshRenderer.enabled = false;

			transform.position = cameraTransform.position;
			transform.rotation = cameraTransform.rotation;
		} else {
			meshRenderer.enabled = true;
		}
	}
}
