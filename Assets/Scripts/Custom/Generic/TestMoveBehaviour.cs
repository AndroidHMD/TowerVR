using UnityEngine;
using System.Collections;

public class TestMoveBehaviour : MonoBehaviour {
	private Vector3 initialPosition;
	
	public float deltaX = 15, deltaY = 15;
	
	void Start()
	{
		initialPosition = transform.position;
	}
	
	void Update () 
	{
		var position = initialPosition;
		
		position.x += deltaX * Mathf.Cos(Time.time);
		position.y += deltaY * Mathf.Sin(Time.time);
		
		transform.position = position;
	}
}
