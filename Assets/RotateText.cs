using UnityEngine;
using System.Collections;

public class RotateText : MonoBehaviour {

	Vector3 camPos;
	Vector3 textPos;

	void Update () {

		camPos = Vector3.Normalize(GameObject.FindGameObjectWithTag("MainCamera").transform.forward);
		textPos = Vector3.Normalize(transform.forward);

		if (Vector3.Dot (camPos, textPos) < 0)
			gameObject.transform.RotateAround (transform.position, Vector3.up, 180);
	}

}

