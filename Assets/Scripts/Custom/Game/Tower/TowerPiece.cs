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
				rb.isKinematic = true;
				rb.detectCollisions = false;
				rb.useGravity = false;
			}
			gameObject.layer = 8;
			gameObject.tag = "newTowerPiece";
			gameObject.GetComponent<Collider>().isTrigger = false;
		}
		
		public override void OnOwnershipRequest(object[] viewAndPlayer)
		{
			PhotonView view = viewAndPlayer[0] as PhotonView;
			PhotonPlayer player = viewAndPlayer[1] as PhotonPlayer;
			
			Debug.Log("OnOwnershipRequest from " + player.ID);
			
			view.TransferOwnership(player.ID);
 		} 
		 
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
			var rb = gameObject.GetComponent<Rigidbody>();
			if (stream.isWriting)
			{
				//We own this player: send the others our data
				stream.SendNext((bool) rb.isKinematic );
				stream.SendNext((bool) rb.detectCollisions );
				stream.SendNext((bool) rb.useGravity );
				stream.SendNext(transform.position);
				stream.SendNext(transform.rotation);
			}
			else
			{
				//Network player, receive data
				rb.isKinematic = (bool)stream.ReceiveNext();
				rb.detectCollisions = (bool)stream.ReceiveNext();
				rb.useGravity = (bool)stream.ReceiveNext();
				transform.position = (Vector3)stream.ReceiveNext();
				transform.rotation = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}