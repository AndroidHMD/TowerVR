using UnityEngine;
using System.Collections;


public class ShowServers : MonoBehaviour, ICardboardGazeResponder  {

	public GameObject cubePrefab;
	private static int positionX = 0;
	private static int positionY = 0;
	private bool showLobbies = true;
	GameObject lobbyCube;

	// Use this for initialization
	void Start () {

		PhotonNetwork.ConnectUsingSettings ("1.0");
		GetComponent<Renderer>().material.color = Color.red;

	}

	public void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby ();

	}
		
	public void SpawnCube(){

		if (showLobbies) {

			foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {

				int i = 0;

				lobbyCube = Instantiate (cubePrefab, new Vector3 (positionX, positionY + 20, 20), transform.rotation) as GameObject;
				//myList.push (lobbyCube);
				lobbyCube.GetComponentInChildren<TextMesh> ().text = room.name;
				lobbyCube.tag = "" + i++  + "";

				positionX += 10;

				if (positionX >= 50) {
					positionX = 0;
					positionY += 10;
				} 
			}
		} else {

			//foreach (GameObject obj in myList)
			//	Destroy(obj);

		}

		showLobbies = showLobbies ? false : true;
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connected ? "Connected" : "Disconnected");
		if (PhotonNetwork.connected)
		{
			GUILayout.Label("Available rooms: " + PhotonNetwork.GetRoomList().Length);    
		}
	}


	#region ICardboardGazeResponder implementation

	// Called when the Cardboard trigger is used, between OnGazeEnter
	/// and OnGazeExit.
	public void OnGazeEnter() {


	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {




	}
	public void OnGazeTrigger() {

		SpawnCube();
	}

	#endregion

}
