using UnityEngine;
using System.Collections;

public class removeTargetChild : MonoBehaviour {

	private int count;


	void Start () {
		count = 0;


	}

	void Update(){

		//Run just one time!
		if (count == 0) {
			foreach (var side in GameObject.FindGameObjectsWithTag("targetSide")) {
				Destroy (side);
				count++;
			}
			Debug.Log ("Removed " + count + " sides.");
		}
	}

}
