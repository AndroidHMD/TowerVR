using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour, ICardboardGazeResponder  {

	public TextMesh text;

	void Start () {
		
		GetComponent<Renderer>().material.color = Color.red;

	}

	public void joinServer(){

		PhotonNetwork.JoinRoom (text.text);


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

		joinServer();
	}

	#endregion

}