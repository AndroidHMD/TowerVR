using UnityEngine;
using System.Collections;


public class Gaze : MonoBehaviour  {

	public void SetGazedAt(bool gazedAt)
	{
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}

	public void OnGazeEnter() {
		SetGazedAt (true);
	}

	public void OnGazeExit() {
		SetGazedAt (false);
	}
}