using UnityEngine;
using System.Collections;

public class RotateAroundUp : MonoBehaviour 
{
	public Vector3 deltaRotationAxis = Vector3.zero;
	public float revolutionsPerSecond = 0.5f;
	
	private Vector3 startPosition;
	
	void Start()
	{
		startPosition = transform.position;
	}
	
	void Update () 
	{
		transform.RotateAround(startPosition + deltaRotationAxis, Vector3.up, 360 * revolutionsPerSecond * Time.deltaTime);
	}
}
