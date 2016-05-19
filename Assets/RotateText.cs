using UnityEngine;
using System.Collections;

public class RotateText : MonoBehaviour {

	Vector3 camPos;
	Vector3 textPos;

	// Use this for initialization
	void Start () {

		camPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		textPos = gameObject.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (Vector3.Dot (camPos, textPos));

		camPos = Vector3.Normalize(GameObject.FindGameObjectWithTag("MainCamera").transform.forward);
		textPos = Vector3.Normalize(gameObject.transform.forward);

		if (Vector3.Dot (camPos, textPos) < 0)
			gameObject.transform.RotateAround (transform.position, Vector3.up, 180);
	}

}

