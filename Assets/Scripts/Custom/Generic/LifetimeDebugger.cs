using UnityEngine;
using System.Collections;

public class LifetimeDebugger : MonoBehaviour {
	public bool enabled = true;
	
	private void log(string str)
	{
		Debug.Log(gameObject.name + ": " + str);
	}
	
	void Awake()
	{
		log("Awake()");
	}
	
	void Start () {
		log("Start()");
	}
	
	void OnDestroy()
	{
		log("OnDestroy()");
	}
}
