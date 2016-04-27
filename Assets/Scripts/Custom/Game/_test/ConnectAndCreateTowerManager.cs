using UnityEngine;
using System.Collections;

public class ConnectAndCreateTowerManager : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("1.0");
	}
	
	void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
		PhotonNetwork.JoinOrCreateRoom("TowerVR", new RoomOptions(), new TypedLobby());
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		
		gameObject.AddComponent<TowerVR.TowerGameManager>();
		gameObject.AddComponent<TowerVR.PlayerTurnObserver>();
	}
}