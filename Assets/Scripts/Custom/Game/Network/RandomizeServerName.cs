/* 
 * Randomizes a name for a new server. Checks if the randomized name is 
 * already used, in that case it randomizes a new one.
 */

using UnityEngine;
using System.Collections;

public class RandomizeServerName : MonoBehaviour {
	public static string[] prefixes = new string[]{
		"Futuristic", "Ancient", "Modern", "Medieval", "Roman", "Egyptian", "Swedish"
	};
	
	public static string[] mainNames = new string[]{
		"Arena", "Playground", "City", "Battlefield", "Forest", "Sandbox", "Mountaintop", "Castle", "Planet", "Solar System"
	};

	// The name of the server
	public static string serverName = "";

	// Sets 'serverName' to a random name and checks if it matches 
	// any of the currently used names. If so it randomizes another name
	// and checks again, and so it goes forever and ever and ever and....
	void RandomServerName(){

		// gets a random string from each string array
		serverName = prefixes[Random.Range(0,prefixes.Length)] + " " + mainNames[Random.Range(0, mainNames.Length)];
	
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
		
			if (serverName.Equals(room.name))
			{
				RandomServerName();
				return;
			}
		}
	}

	// gets a random string and sets the components text to it
	void Start() {

		RandomServerName ();
		GetComponent<TextMesh> ().text = serverName;
	}
}
