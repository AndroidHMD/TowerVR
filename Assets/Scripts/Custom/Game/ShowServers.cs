using UnityEngine;
using System.Collections;


public class ShowServers : MonoBehaviour   {

	public GameObject cubePrefab;
	private static int positionY = 20;
	GameObject lobbyCube;

	void Start () {
		PhotonNetwork.ConnectUsingSettings ("1.0");
	}

	public void OnConnectedToMaster() {
		PhotonNetwork.JoinLobby ();
	}
		
	public void SpawnCube(){

		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {

			lobbyCube = Instantiate (cubePrefab, new Vector3 (0, positionY, 20), transform.rotation) as GameObject;
			lobbyCube.GetComponentInChildren<TextMesh> ().text = room.name;

			positionY += 10;
		}
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connected ? "Connected" : "Disconnected");

		if (PhotonNetwork.connected)
		{
			GUILayout.Label("Available rooms: " + PhotonNetwork.GetRoomList().Length);    
		}
	}
}
