/* 
 * Randomizes a name for a new server. Checks if the randomized name is 
 * already used, in that case it randomizes a new one.
 */

using UnityEngine;
using System.Collections;

public class RandomizeServerName : MonoBehaviour {

	string[] firstName = new string[]{"Big", "Hairy", "Small", "Ugly", "Old", };
	string[] middleName = new string[]{"Yellow", "Red", "Blue", "Angry", "Bald"};
	string[] lastName = new string[]{"Man", "Bear", "Pig", "Bunny", "Penis"};
	static public string serverName = "";


	void RandomServerName(){

		serverName = firstName[Random.Range(0,4)] + middleName[Random.Range(0,4)] + lastName[Random.Range(0,4)];
	
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
		
			if (serverName == room.name)
				RandomServerName();
		}

	}

	void Start() {

		RandomServerName ();
		GetComponent<TextMesh> ().text = serverName;
	}
}
