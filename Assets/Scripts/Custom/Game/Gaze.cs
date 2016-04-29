using UnityEngine;
using System.Collections;


public class Gaze : MonoBehaviour, ICardboardGazeResponder  {


	public void SetGazedAt(bool gazedAt)
	{
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}


	#region ICardboardGazeResponder implementation

	// Called when the Cardboard trigger is used, between OnGazeEnter
	/// and OnGazeExit.
	public void OnGazeEnter() {
		SetGazedAt (true);

	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {

		SetGazedAt (false);


	}
	public void OnGazeTrigger() {


	}

	#endregion

}