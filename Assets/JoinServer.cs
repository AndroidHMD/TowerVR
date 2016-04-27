using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour {

	public TextMesh text;
	public int LevelIndex;

	public void joinServer(){
		PhotonNetwork.JoinRoom (text.text);
		ChangeScene.ChangeToScene (LevelIndex);
	}
}