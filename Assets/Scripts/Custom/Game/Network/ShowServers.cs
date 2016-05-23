using UnityEngine;
using System.Collections;

public class ShowServers : MonoBehaviour   {

	public GameObject cubePrefab;
	public GameObject noServers;
	private static int positionY = 0;
	GameObject lobbyCube;
	GameObject serverText;
	bool show = true;

	// Connects to PhotonNetwork
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("1.0");
	}

	// Joins the lobby
	public void OnConnectedToMaster() {
		PhotonNetwork.JoinLobby ();
	}
		
	// Spawns a cube for each room in Photon and displays them in a list with their name 
	public void SpawnCube(){

		if (show) {

			int count = 0;

			foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {

				lobbyCube = Instantiate (cubePrefab, new Vector3 (-10, positionY, 0), Quaternion.Euler(-90, 90, 0)) as GameObject;
				lobbyCube.GetComponentInChildren<TextMesh> ().text = room.name;
				lobbyCube.tag = "lobbyCube";

				positionY -= 7;
				count++;
			}

			if (count == 0) {
			
				serverText = Instantiate (noServers, new Vector3 (0, -5, 0), Quaternion.identity) as GameObject;

			}
				
			positionY = 0;
			count = 0;
			show = false;

		} else 
		{

			Destroy (serverText);

			GameObject[] cubes = GameObject.FindGameObjectsWithTag ("lobbyCube");

			foreach (GameObject cube in cubes)
				Destroy (cube);

			show = true;
		}

	}

	// Displays number of rooms
	void OnGUI()
	{
		GUILayout.Label("");
		GUILayout.Label(PhotonNetwork.connected ? "Connected" : "Disconnected");

		if (PhotonNetwork.connected)
		{
			GUILayout.Label("Available rooms: " + PhotonNetwork.GetRoomList().Length);    
		}
	}
}
