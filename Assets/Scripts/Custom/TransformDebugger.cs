using UnityEngine;
using System.Collections;

public class TransformDebugger : MonoBehaviour {
	public bool useCustomDebugInterval = false;
	public float updateIntervalSeconds = 1.0f;

	public bool debugPosition;
	public bool debugRotation;
	public bool debugScale;

	void debugTransform() {
		if (!debugPosition && !debugRotation && !debugScale) {
			return;
		}
		
		string debugMessage = gameObject.name + " [";
		
		if (debugPosition) {
			debugMessage += "position=" + gameObject.transform.position;
		}
		
		if (debugRotation) {
			debugMessage += " rotation=" + gameObject.transform.rotation;
		}
		
		if (debugScale) {
			debugMessage += " scale=" + gameObject.transform.localScale;
		}
		
		debugMessage += "]";
		
		Debug.Log(debugMessage);
	}

	void Start () {
		if (useCustomDebugInterval) {
			InvokeRepeating("debugTransform", 0, updateIntervalSeconds);
		}
	}

	void Update () {
		if (useCustomDebugInterval) {
			return;
		}

		debugTransform();
	}
}