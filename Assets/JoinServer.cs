using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour {

	public TextMesh text;

	public void joinServer(){
		PhotonNetwork.JoinRoom (text.text);
	}
}