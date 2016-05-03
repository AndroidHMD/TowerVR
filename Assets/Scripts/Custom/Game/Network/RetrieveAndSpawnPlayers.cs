using UnityEngine;
using System.Collections;

/**
* Retrieves players from active room and spawns them in the scene
*/
public class RetrieveAndSpawnPlayers : Photon.MonoBehaviour 
{
	public string playerPrefabName;
	public bool debugGuiOn = false;
	private string players = "";
	private GameObject thisPlayer;
	
	// wip
	void Start()
	{
		SpawnPlayer();
		UpdatePlayerDebugList();
	}
	
	/**
	* Called by Photon when player joins a room, whether created or not
	*/
	void OnJoinedRoom() 
	{
		
	}
	
	void OnPhotonPlayerConnected()
 	{
		 UpdatePlayerDebugList();
		//  Debug.Log("DEBUG: Other player joined");
	}
	
	void OnPhotonPlayerDisconnected()
 	{
		 UpdatePlayerDebugList();
		//  Debug.Log("DEBUG: Other player disconnected");
	}
	
	/**
	* Associates prefab in Resources/ to the player and loads into game for all in room
	*/
	void SpawnPlayer()
	{
		thisPlayer = PhotonNetwork.Instantiate( playerPrefabName, Vector3.zero, Quaternion.identity, 0 );
		// Debug.Log("DEBUG: New player instantiated, ");
	}
	
	/**
	* Updates list of current players in room for debug purpose
	*/
	void UpdatePlayerDebugList()
	{
		players = PhotonNetwork.player.ToString() + "(you) ";
		foreach(PhotonPlayer player in PhotonNetwork.otherPlayers)
		{
			players += player.ToString() + " ";
		}
	}
	
	/**
	* Prints player list to text in the upper right corner
	*/
	void OnGUI()
	{	
		if (debugGuiOn) {
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