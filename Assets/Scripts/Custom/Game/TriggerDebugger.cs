using UnityEngine;
using System.Collections;

public class TriggerDebugger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter [this=" + gameObject + ", enteringCollider" + other + "]");
	}
}