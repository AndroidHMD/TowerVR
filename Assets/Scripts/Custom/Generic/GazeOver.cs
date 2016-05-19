using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * A class for switching scenes.
 * How to use: Make sure your object has a Collider. 
 * Add this script and set which scene it should chnage to with index (from build settings).
 * Add EventTrigger-component and use Pointer Down, Pointer Enter, Pointer Exit for the functions below.
 * Check the box for the loadSceneForAllPlayers variable to use Photons functionality to sync the loaded scene.
 **/

public class GazeOver : MonoBehaviour {

    private Color objColor;
	public bool loadSceneForAllPlayers = false;
    public int levelIndex;
	private string[] levelNames;

    //Variables for loading the level
    public string LevelName;
    public string LevelSkybox;


	void Start()
	{
		if (loadSceneForAllPlayers)
			PhotonNetwork.automaticallySyncScene = true;

		// These are manually kept as the correct names of the scenes in the right order
		// Reason being that there is no way to programatically get the name of a scene from its index
		levelNames = new string[4]{"Menu_HostJoin", "Menu_ChooseBackdrop", "Menu_GameRoom", "TowerStacker"};
	}
	//Function for switching scene. 	
	public void SwitchScene(){
		//TODO: This makes FadeOut work, but tracking doesn't because you have to use Keep ARcamera alive
		//No solution found yet, you probably have to load ARCamera and CardboardMain in a seperate scene befora any objects and use KeepARCameraAlive
		//DontDestroyOnLoad (GameObject.Find ("CardboardMain"));

		//fade out and load new level
		float fadeTime = GameObject.Find("SceneLogic").GetComponent<Fading>().BeginFade(1);
		StartCoroutine(wait(fadeTime*2));
		
		// If level index is set to first scene, 
		// leave room and go there disregarding other players
		if (levelIndex == 0)
		{
			PhotonNetwork.LeaveRoom();
			SceneManager.LoadScene(levelIndex);
			return;
		}
		
		if (loadSceneForAllPlayers)
		{
			PhotonNetwork.LoadLevel(levelNames[levelIndex]);
			Debug.Log ("Change to scene " + levelNames[levelIndex]);
		}
		
		else
		{
			SceneManager.LoadScene(levelIndex);		
			Debug.Log ("Change to scene " + levelIndex);
		}
		
	}

    //Function for loading prefabs/materials into a scene
    public void LoadScene()
    {
        SpawnSelectedLevel.LoadedLevel = LevelName;
        SpawnSelectedLevel.LoadedSkybox = LevelSkybox;
    }

	//When you look at an object, change color to red
    public void OnPointerEnter()
    {
		//Save the original color first
		objColor = gameObject.GetComponent<Renderer>().material.color;

		//Change color to red
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

	/// <summary>
	/// When you look away again, change back to original color
	/// </summary>
    public void OnPointerExit()
    {
        gameObject.GetComponent<Renderer>().material.color = objColor;
    }
	
	//Wait until the fading is done until we change scene
	private IEnumerator wait(float waitSec) {
		yield return new WaitForSeconds(waitSec);
	}

}
