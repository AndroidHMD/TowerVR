﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*
 * A class for switching scenes.
 * How to use: Make sure your object has a Collider. 
 * Add this script and set which scene it should chnage to with index (from build settings).
 * Add EventTrigger-component and use Pointer Down, Pointer Enter, Pointer Exit for the functions below.
 */

public class GazeOver : MonoBehaviour {

    private Color objColor;
    public int LevelIndex;

    //Variables for loading the level
    public string LevelName;
    public string LevelMaterial1;
    public string LevelSkybox;

    //Function for switching scene. 	
    public void SwitchScene(){
		//TODO: This makes FadeOut work, but tracking doesn't because you have to use Keep ARcamera alive
		//No solution found yet
		//DontDestroyOnLoad (GameObject.Find ("CardboardMain"));

		//fade out and load new level
		float fadeTime = GameObject.Find("SceneLogic").GetComponent<Fading>().BeginFade(1);
		StartCoroutine(wait(fadeTime*2));

		SceneManager.LoadScene(LevelIndex);
		Debug.Log ("Change to scene: " + LevelIndex);
	}

    //Function for loading prefabs/materials into a scene
    public void LoadScene()
    {
        SpawnSelectedLevel.LoadedLevel = LevelName;
        SpawnSelectedLevel.LoadedMaterial1 = LevelMaterial1;
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

	//When you look away again, change back to original color
    public void OnPointerExit()
    {
        gameObject.GetComponent<Renderer>().material.color = objColor;
    }
	
	//Wait until the fading is done until we change scene
	private IEnumerator wait(float waitSec) {
		yield return new WaitForSeconds(waitSec);
	}

}
