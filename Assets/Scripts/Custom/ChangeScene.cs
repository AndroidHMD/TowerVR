using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	
	public static void ChangeToScene (int sceneIndex) {
        Application.LoadLevel(sceneIndex);
	}
}
