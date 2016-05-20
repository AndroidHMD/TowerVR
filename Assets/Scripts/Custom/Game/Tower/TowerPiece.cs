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
			gameObject.tag = "newTowerPiece";
			gameObject.GetComponent<Collider>().isTrigger = false;
		}
		
//		public void OnOwnershipRequest(object[] viewAndPlayer)
//		{
//			PhotonView view = viewAndPlayer[0] as PhotonView;
//			PhotonPlayer player = viewAndPlayer[1] as PhotonPlayer;
//			
//			ScreenLog.Log("OnOwnershipRequest from " + player.ID);
//			
//			view.TransferOwnership(player.ID);
// 		} 
		 
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
			var rb = gameObject.GetComponent<Rigidbody>();
			var col = gameObject.GetComponent<Collider>();
			if (stream.isWriting)
			{
				//Debug.Log("Rigidbody states changed" +);
				//We own this player: send the others our data
				stream.SendNext((bool) rb.isKinematic );
				stream.SendNext((bool) rb.detectCollisions );
				stream.SendNext((bool) rb.useGravity );
				stream.SendNext((bool) col.isTrigger);
				stream.SendNext((int) gameObject.layer);
				//stream.SendNext(transform.position);
				//stream.SendNext(transform.rotation);
			}
			else
			{
				//Network player, receive data
				rb.isKinematic = (bool)stream.ReceiveNext();
				rb.detectCollisions = (bool)stream.ReceiveNext();
				rb.useGravity = (bool)stream.ReceiveNext();
				col.isTrigger = (bool)stream.ReceiveNext();
				gameObject.layer = (int)stream.ReceiveNext();
				//transform.position = (Vector3)stream.ReceiveNext();
				//transform.rotation = (Quaternion)stream.ReceiveNext();
			}
		}
		//Testa att kommentera bort hela funktionen!
		//Felen nu är att:
		//1 - Ett block kan slutas renderas när man väljer ett annat (endast lokalt, den kommer tillbaka när man placerar biten)
		//2 - Klienten känner inte sina egna bitar (eller masterns... Synka layer!?!?!?!? KAn vara det som gör att den inte känner av med boxcast!)
		//Isf är det bara det jävla renderaren som behöver fixas
	}
}