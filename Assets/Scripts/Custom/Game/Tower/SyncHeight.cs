using UnityEngine;
using System.Collections;

/** 
* Syncs if the plattform should increase height
*
*/

public class SyncHeight : Photon.PunBehaviour {

	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//Master says it's time to move up! Master client has changed the transform
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			stream.SendNext(transform.position);
		}
		else
		{
			//Network player, receive data
			transform.position = (Vector3)stream.ReceiveNext();
		}
	}
}
