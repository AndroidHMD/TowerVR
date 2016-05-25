using UnityEngine;
using System.Collections;

namespace TowerVR
	{
	/**
	 * Marker class for a Tower Piece.
	 * 
	 * This component should be attached to a GameObject that has a mesh, a mesh collider and a material.
	 * The GameObject should also have a Rigidbody and a PhotonView, that syncs GameObject's Transform and this script. 
	 * The PhotonView should also have settings; Owner: Takeover, Observe option: Reliable, Transform serialization: Pos & Rot
	 * */
	public class TowerPiece : Photon.PunBehaviour
	{
		void Start()
		{
			var rb = gameObject.GetComponent<Rigidbody>();
			
			if(!PhotonNetwork.isMasterClient)
			{
				Destroy(rb);
			}
			if (rb != null)
			{
				rb.isKinematic = true;
				rb.detectCollisions = false;
				rb.useGravity = false;
			}
			
			gameObject.tag = "newTowerPiece";
			gameObject.GetComponent<MeshCollider>().isTrigger = false;
			//gameObject.GetComponent<MeshCollider>().convex = false;
		}
		 
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
			var rb = gameObject.GetComponent<Rigidbody>();
			var col = gameObject.GetComponent<MeshCollider>();
			if (stream.isWriting)
			{
				//We own this player: send the others our data
				//stream.SendNext((bool) rb.isKinematic );
				//stream.SendNext((bool) rb.detectCollisions );
				//stream.SendNext((bool) rb.useGravity );
				//stream.SendNext((bool) col.convex);
				stream.SendNext((bool) col.isTrigger);
				stream.SendNext((int) gameObject.layer);
			}
			else
			{
				//Network player, receive data
				//rb.isKinematic = (bool)stream.ReceiveNext();
				//rb.detectCollisions = (bool)stream.ReceiveNext();
				//rb.useGravity = (bool)stream.ReceiveNext();
				//col.convex = (bool)stream.ReceiveNext();
				col.isTrigger = (bool)stream.ReceiveNext();
				gameObject.layer = (int)stream.ReceiveNext();
			}
		}
	}
}