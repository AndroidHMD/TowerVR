using UnityEngine;
using System.Collections;

/// <summary>
/// Retrieves players from active room and spawns them in scene
/// </summary>
public class GetAndSpawnPlayers : Photon.MonoBehaviour 
{
	public string playerPrefabName;
	public bool guiOn = true;
	private string players;
	
	void Start()
	{
		//players = "";
	}
	
	/// Called by Photon when player joins a room, whether created or not
	void OnJoinedRoom() 
	{
		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
			players += player.ToString() + " ";
		}
		Debug.Log("players: " + players);
	}
	
	void Spawn()
	{
		// foreach (var player in PhotonNetwork.otherPlayers)
        // {
        //     PhotonNetwork.Instantiate( playerPrefabName, Vector3.zero, Quaternion.identity, 0 );
        // }
	}
	
	/// Prints player list to text in the upper right corner
	void OnGUI()
	{	
		if (guiOn) {
			int w = Screen.width, h = Screen.height;
			GUIStyle style = new GUIStyle();
			Rect rect = new Rect(0, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperRight;
			style.fontSize = h * 2 / 100;
			style.normal.textColor = new Color (0.8f, 1.0f, 1.0f, 1.0f);
			string text = "Players: " + players;
			GUI.Label(rect, text, style);
		}
	}
}