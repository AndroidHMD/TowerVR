using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 *	A transition manager (singleton) that encapsulates the scene loading behaviour.
 *
 *	Subclasses should expose methods that load specific scenes.
 */
public abstract class SceneTransitionManager<T> : Singleton<T> where T : MonoBehaviour {
	protected void LoadScene(int sceneBuildIndex)
	{
		// Custom behaviour for loading a scene, maybe animate dissolving of current scene as discussed?
		SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
	}
}