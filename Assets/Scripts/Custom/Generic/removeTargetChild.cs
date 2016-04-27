using UnityEngine;
using System.Collections;

/**
 *	Removes all sides to the cube so we doesn't render them, or that they occludes objects.
 */

public class removeTargetChild : MonoBehaviour {

	private int count;
	private bool removed;


	void Start () {
		count = 0;
		removed = false;
	}

	void Update(){

		//Run just one time!
		if (count == 0 && !removed) {
			foreach (var side in GameObject.FindGameObjectsWithTag("targetSide")) {
				Destroy (side);
				count++;
			}
			Debug.Log ("Removed " + count + " sides.");
			removed = true;
		}
	} 

}
