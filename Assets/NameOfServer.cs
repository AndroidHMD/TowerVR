using UnityEngine;
using System.Collections;

public class NameOfServer : MonoBehaviour {

	void OnJoinedRoom(){
	
		Debug.Log (PhotonNetwork.room.name);
		GetComponent<TextMesh> ().text = PhotonNetwork.room.name;
	}
}
