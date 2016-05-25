using UnityEngine;
using System.Collections;

public class ToggleVR : MonoBehaviour {

	static bool enable = true;

	void Start(){
		
		GameObject.Find ("CardboardMain").GetComponent<Cardboard> ().VRModeEnabled = enable;
	
	}

	public void Toggle(){

		if (enable)
			enable = false;
		else
			enable = true;

		GameObject.Find ("CardboardMain").GetComponent<Cardboard> ().VRModeEnabled = enable;
	}
}
