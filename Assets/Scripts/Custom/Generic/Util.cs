using UnityEngine;
using System.Collections;

/**
 * Collection of utility functions
 * */
public static class Util
{
	/**
	 * Helper function for getting components. Usage:
	 * 
	 * Transform transform;
	 * 
	 * if (Util.TryGetComponent<Transform>(gameObject, transform))
	 * {
	 * 		// The GameObject had a Transform, yay!!
	 * }
	 * */
	public static bool TryGetComponent<T>(GameObject obj, out T component) where T : MonoBehaviour
	{
		component = obj.GetComponent<T>();

		return component != null;
	}
}