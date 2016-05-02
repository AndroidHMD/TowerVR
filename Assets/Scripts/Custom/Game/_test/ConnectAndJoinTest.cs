using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ConnectAndJoinTest : MonoBehaviour {
	public int sceneIndex;
	public int minimumPlayers = 1;
	
	void Start () {
		Debug.Log("Start");
		PhotonNetwork.ConnectUsingSettings("1.0");
	}
	
	void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
		PhotonNetwork.JoinOrCreateRoom("SwagBOI", new RoomOptions(), new TypedLobby());
	}
	
	void Update()
	{
		if (PhotonNetwork.playerList.Length >= minimumPlayers)
		{
			SceneManager.LoadScene(sceneIndex);
		}
	}
}
