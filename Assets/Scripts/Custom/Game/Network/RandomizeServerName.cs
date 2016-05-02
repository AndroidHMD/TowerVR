/* 
 * Randomizes a name for a new server. Checks if the randomized name is 
 * already used, in that case it randomizes a new one.
 */

using UnityEngine;
using System.Collections;

public class RandomizeServerName : MonoBehaviour {

	// String arrays containing different strings 
	string[] firstName = new string[]{"Big", "Hairy", "Small", "Ugly", "Old", };
	string[] middleName = new string[]{"Yellow", "Red", "Blue", "Angry", "Bald"};
	string[] lastName = new string[]{"Man", "Bear", "Pig", "Bunny", "Penis"};

	// The name of the server
	static public string serverName = "";

	// Sets 'serverName' to a random name and checks if it matches 
	// any of the currently used names. If so it randomizes another name
	// and checks again, and so it goes forever and ever and ever and....
	void RandomServerName(){

		// gets a random string from each string array
		serverName = firstName[Random.Range(0,4)] + middleName[Random.Range(0,4)] + lastName[Random.Range(0,4)];
	
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
		
			if (serverName == room.name)
				RandomServerName();
		}

	}

	// gets a random string and sets the components text to it
	void Start() {

		RandomServerName ();
		GetComponent<TextMesh> ().text = serverName;
	}
}
