using UnityEngine;
using System.Collections;


public class HostGame : MonoBehaviour, ICardboardGazeResponder  {

	public void Host(){
	
	}


	#region ICardboardGazeResponder implementation

	// Called when the Cardboard trigger is used, between OnGazeEnter
	/// and OnGazeExit.
	public void OnGazeEnter() {


	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {




	}
	public void OnGazeTrigger() {

		Host();
	}

	#endregion

}