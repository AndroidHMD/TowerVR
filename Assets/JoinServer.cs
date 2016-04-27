using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JoinServer : MonoBehaviour {

	public TextMesh text;
	public int LevelIndex;

	public void joinServer(){
		PhotonNetwork.JoinRoom (text.text);
		SceneManager.LoadScene (LevelIndex);
	}
}