using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	
	public static void ChangeToScene (int sceneIndex) {
		SceneManager.LoadScene (sceneIndex);
	}
}
