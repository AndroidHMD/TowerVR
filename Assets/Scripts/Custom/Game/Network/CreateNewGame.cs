using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateNewGame : MonoBehaviour {
	
	public int levelIndex;

	void Start () {
		PhotonNetwork.ConnectUsingSettings ("1.0");
	}

	public void OnConnectedToMaster() {
		PhotonNetwork.JoinLobby ();
	}

	public void StartNewGame() {

		RoomOptions options = new RoomOptions ();
		options.maxPlayers = (byte)SetMaxPlayers.maxPlayers;
		string serverName = RandomizeServerName.serverName;
		PhotonNetwork.CreateRoom(serverName, options, TypedLobby.Default);
		//SceneManager.LoadScene(levelIndex);

	}
}
