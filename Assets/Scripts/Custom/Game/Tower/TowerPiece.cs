using UnityEngine;
using System.Collections;

namespace TowerVR
	{
	/**
	 * Marker class for a Tower Piece.
	 * 
	 * This component should be attached to a GameObject that has a mesh, a mesh collider and a material.
	 * */
	public class TowerPiece : Photon.PunBehaviour
	{
		void Start()
		{
			var rb = gameObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.isKinematic = false;
				rb.detectCollisions = false;
			}
			gameObject.layer = 8;
		}
		
		void OnOwnershipRequest(object[] viewAndPlayer)
		{
			PhotonView view = viewAndPlayer[0] as PhotonView;
			PhotonPlayer player = viewAndPlayer[1] as PhotonPlayer;
			
			Debug.Log("OnOwnershipRequest from " + player.ID);
			
			view.TransferOwnership(player.ID);
 		} 
	}
}