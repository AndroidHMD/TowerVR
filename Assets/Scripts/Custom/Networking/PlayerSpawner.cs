using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	public string playerPrefabName;

	void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom()");
		CreatePlayerObject();
	}

	void CreatePlayerObject()
	{
		Debug.Log("CreatePlayerObject()");
		var newPlayerObject = PhotonNetwork.Instantiate( playerPrefabName, Vector3.zero, Quaternion.identity, 0 );
		Debug.Log(newPlayerObject);
	}
}
