using UnityEngine;
using System.Collections;

public class FirstMenuTransitionManager : SceneTransitionManager<FirstMenuTransitionManager>
{
	public int chooseBackdropSceneBuildIndex;
	public int loadGameRoomSceneBuildIndex;

	public void LoadChooseBackdropScene()
	{
		Debug.Log("LoadChooseBackdropScene()");
		LoadScene(chooseBackdropSceneBuildIndex);
	}

	public void LoadGameRoomScene()
	{
		Debug.Log("LoadGameRoomScene()");
		LoadScene(loadGameRoomSceneBuildIndex);
	}
}

/**
 * Usage
 * 
 * void OnButtonClicked()
 * {
 * 		FirstMenuTransitionManager.Instance.LoadChooseBackdropScene();
 * }
 */