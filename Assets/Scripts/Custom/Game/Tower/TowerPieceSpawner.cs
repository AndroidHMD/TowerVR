using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerPieceSpawner : MonoBehaviour
{
	public List<Material> materials;
	public List<TowerPieceInfo> towerPieceInfos;

	private IList<TowerPiece> towerPieces = new List<TowerPiece> ();

	public void Network_SpawnTowerPiece (Vector3 position, Vector3 eulerAngles)
	{
		var materialIndex = Random.Range (0, materials.Capacity);
		var towerPieceInfoIndex = Random.Range (0, towerPieceInfos.Capacity);

		// Write info about the new tower piece's material, transform etc.
		string[] towerPieceOptions = new string[8];
		towerPieceOptions [0] = materialIndex.ToString ();
		towerPieceOptions [1] = towerPieceInfoIndex.ToString ();

		towerPieceOptions [2] = position.x.ToString ();
		towerPieceOptions [3] = position.y.ToString ();
		towerPieceOptions [4] = position.z.ToString ();

		towerPieceOptions [5] = position.x.ToString ();
		towerPieceOptions [6] = position.y.ToString ();
		towerPieceOptions [7] = position.z.ToString ();

		var options = RaiseEventOptions.Default;

		// Send to all users (including self)
		options.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;

		PhotonNetwork.RaiseEvent (NetworkEventCodes.SPAWN_TOWER_PIECE,
			towerPieceOptions, true, options);
	}

	private void OnEventHandler (byte eventCode, object content, int senderId)
	{ 
		Debug.Log ("OnEventHandler [eventCode=" + eventCode + " content=" + content + " senderId=" + senderId + "]"); 

		switch (eventCode)
		{
			case NetworkEventCodes.SPAWN_TOWER_PIECE:
				HandleSpawnTowerPieceEvent (content);
				break;
		}
	}

	private void HandleSpawnTowerPieceEvent (object content)
	{
		string[] towerPieceOptions = (string[])content;

		int materialIndex, towerPieceInfoIndex;
		float posX, posY, posZ, rotX, rotY, rotZ;

		if (int.TryParse (towerPieceOptions [0], out materialIndex) &&
		    int.TryParse (towerPieceOptions [1], out towerPieceInfoIndex) &&
		    float.TryParse (towerPieceOptions [2], out posX) &&
		    float.TryParse (towerPieceOptions [3], out posY) &&
		    float.TryParse (towerPieceOptions [4], out posZ) &&
		    float.TryParse (towerPieceOptions [5], out rotX) &&
		    float.TryParse (towerPieceOptions [6], out rotY) &&
		    float.TryParse (towerPieceOptions [7], out rotZ))
		{
			Vector3 position = new Vector3 (posX, posY, posZ);
			Vector3 eulerAngles = new Vector3 (rotX, rotY, rotZ);

			Local_SpawnTowerPiece (materialIndex,
				towerPieceInfoIndex,
				position,
				eulerAngles);
		}

		else 
		{
			Debug.LogError ("Received wrongly formatted SPAWN_TOWER_PIECE event.");
		}
	}

	private void Local_SpawnTowerPiece (int materialIndex, int towerPieceInfoIndex, Vector3 position, Vector3 eulerAngles)
	{
		var towerPieceInfo = towerPieceInfos [towerPieceInfoIndex];
		var material = materials [materialIndex];

		var tp = TowerPiece.Create (towerPieceInfo, material);
		towerPieces.Add (tp);
	}

	private void Spawn()
	{
		Debug.Log("Spawn()");
		Network_SpawnTowerPiece(Vector3.zero, Vector3.zero);
	}
		
	void Awake ()
	{
		Debug.Log("Awake()");
		PhotonNetwork.OnEventCall += OnEventHandler;

		PhotonNetwork.ConnectUsingSettings("1.0");
	}

	public void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster()");
		PhotonNetwork.JoinOrCreateRoom("TowerVR", new RoomOptions(), new TypedLobby());
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom()");
		if (PhotonNetwork.isMasterClient)
		{
			InvokeRepeating("Spawn", 1.0f, 1.0f);	
		}
	}
}