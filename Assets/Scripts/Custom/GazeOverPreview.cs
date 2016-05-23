using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GazeOverPreview : MonoBehaviour {

/**
 * This script is based on the GazeOver script but it has some added features for the preview functionality. 
 * Therefore this scripts should ONLY be put on the level miniature objects.
 * The skybox images needs to be named as described in the comment for the "public string skyboxIdentifier" below.
 * All skybox images need to be placed in the Assets/Resources folder.
 *
 * Comments from the GazeOver script:   
 * A class for switching scenes.
 * How to use: Make sure your object has a Collider. 
 * Add this script and set which scene it should chnage to with index (from build settings).
 * Add EventTrigger-component and use Pointer Down, Pointer Enter, Pointer Exit for the functions below.
 * Check the box for the loadSceneForAllPlayers variable to use Photons functionality to sync the loaded scene.
 */

    private Color objColor;
    public bool loadSceneForAllPlayers = false;
    public int levelIndex;

    //Changed the variable name from levelNames[] to sceneNames[] in order to avoid confusion.
    private string[] sceneNames;

    public string levelName;
    public string levelSkybox;


    //    ADDED VARIABLES FOR THE PREVIEW    //

    //Set to true if the object is being gazed on.
    public bool isGazedOn = false;

    //Name the sides in the skybox as: "frontImage" + skyboxIdentifier, "backImage" + skyboxIdentifier,
    //"leftImage" + skyboxIdentifier, "rightImage" + skyboxIdentifier, "upImage" + skyboxIdentifier, and "downImage" + skyboxIdentifier (without spaces).
    public string skyboxIdentifier;

    //This Object is the empty parent obejct of the level that should be previewed.
    //This object is loaded from the Resources folder via the string "levelname".
    //levelName is set in the editor.
    private GameObject levelParent;

    //This material is created using the custom shader "skyboxTransition" located in Assets/shaders.
    public Material blendedSkybox;

    //Value between 0 and 1. 0 gives the first skybox, 1 gives the second skybox. Any value in between gives a blend of the two skyboxes.
    private float blendFactor = 0;


    void Start()
    {
        if (loadSceneForAllPlayers)
            PhotonNetwork.automaticallySyncScene = true;

        // These are manually kept as the correct names of the scenes in the right order
        // Reason being that there is no way to programatically get the name of a scene from its index
        sceneNames = new string[4] { "Menu_HostJoin", "Menu_ChooseBackdrop", "Menu_GameRoom", "TowerStacker" };

        blendedSkybox.SetFloat("_Blend", blendFactor);

        //Set the skybox of the scene to the blendedSkybox. 
        RenderSettings.skybox = blendedSkybox;

        //Load the Level-Object from the Assets/Resources folder using the string "levelName"
        levelParent = (GameObject)Instantiate(Resources.Load(levelName));

        //Adds the script ComponentPreview to all children of the levelParent object. Makes preview possible.
        for (int childIndex = 0; childIndex < levelParent.transform.childCount; childIndex++)
        {
            Transform child = levelParent.transform.GetChild(childIndex);

            child.gameObject.AddComponent<ComponentPreviewer>();
            child.gameObject.GetComponent<ComponentPreviewer>().triggerObject = gameObject;
        }
    }


    void Update()
    {
    //if the object is being gazed on: blend to the preview-level skybox.
    //else: blend to the skybox of the current scene.
        if (isGazedOn)
        {
            if (blendFactor < 1f)
                skyboxBlenderIn();
        }
        else
        {
            if (blendFactor > 0f)
                skyboxBlenderOut();
        }
    }

    //Sets the second skybox images. The first skybox has to be set in the blended material in the editor since it will be the same for all miniature objects. 
    //(The first skybox is the skybox of the current scene)
    //(The second skybox is the skybox for the level to be previewed)
    void SetSkybox()
    {
        blendedSkybox.SetTexture("_FrontTex2", (Texture)Resources.Load("frontImage" + skyboxIdentifier, typeof(Texture)));
        blendedSkybox.SetTexture("_BackTex2", (Texture)Resources.Load("backImage" + skyboxIdentifier, typeof(Texture)));
        blendedSkybox.SetTexture("_LeftTex2", (Texture)Resources.Load("leftImage" + skyboxIdentifier, typeof(Texture)));
        blendedSkybox.SetTexture("_RightTex2", (Texture)Resources.Load("rightImage" + skyboxIdentifier, typeof(Texture)));
        blendedSkybox.SetTexture("_UpTex2", (Texture)Resources.Load("upImage" + skyboxIdentifier, typeof(Texture)));
        blendedSkybox.SetTexture("_DownTex2", (Texture)Resources.Load("downImage" + skyboxIdentifier, typeof(Texture)));
    }

    
    //Decreases the value of of the blend factor -> goes towards the first skybox (skybox of the current scene). 
    void skyboxBlenderOut()
    {
        blendFactor -= Time.deltaTime * 0.8f;

        blendedSkybox.SetFloat("_Blend", blendFactor);
    }


    //Increases the value of of the blend factor -> goes towards the second skybox (skybox of the preview-level).
    void skyboxBlenderIn()
    {
        if (blendFactor < 1f)
        {
            blendFactor += Time.deltaTime * 0.8f;
        }
        blendedSkybox.SetFloat("_Blend", blendFactor);
    }

    //Function for switching scene. 	
    public void SwitchScene()
    {
        //TODO: This makes FadeOut work, but tracking doesn't because you have to use Keep ARcamera alive
        //No solution found yet, you probably have to load ARCamera and CardboardMain in a seperate scene befora any objects and use KeepARCameraAlive
        //DontDestroyOnLoad (GameObject.Find ("CardboardMain"));

        //fade out and load new level
        float fadeTime = GameObject.Find("SceneLogic").GetComponent<Fading>().BeginFade(1);
        StartCoroutine(wait(fadeTime * 2));

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
            PhotonNetwork.LoadLevel(sceneNames[levelIndex]);
            Debug.Log("Change to scene " + sceneNames[levelIndex]);
        }

        else
        {
            SceneManager.LoadScene(levelIndex);
            Debug.Log("Change to scene " + levelIndex);
        }

    }

    //Function for loading prefabs/materials into a scene
    public void LoadScene()
    {
        SpawnSelectedLevel.LoadedLevel = levelName;
        SpawnSelectedLevel.LoadedSkybox = levelSkybox;
    }

    //When you look at an object, change color to red.
    public void OnPointerEnter()
    {
        SetSkybox();

        isGazedOn = true;

        /* Uncomment if you want to change the color of the GazedOn object. Same goes for the OnPointerExit() function.
        objColor = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        */
    }

    //When you look away again, change back to original color.
    public void OnPointerExit()
    {
        isGazedOn = false;

        //gameObject.GetComponent<Renderer>().material.color = objColor;
    }

    //Wait until the fading is done until we change scene
    private IEnumerator wait(float waitSec)
    {
        yield return new WaitForSeconds(waitSec);
    }

}
