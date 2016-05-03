using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpawnSelectedLevel : MonoBehaviour {

    /** Assume the level selection scene is "Scene 0" and the game scene is "Scene 1".
     *
     *  Add this script to the scene manager in the game scene. (Scene 1)
     *  This scripts needs info form another script on the level-selection object ( in Scene 0).
     *  The public static string variables in this scripts should be assigned when clicking the level-selection object (in another script in scene 0).
     *      -EX: OnClick(){SpawnSelectedLevel.LoadedLevel = LevelName;} //When clicking (selecting the level), set the name of the prefab level to load.
     *
     *  See example in the script "MouseOver.cs" in the Unity/Scripts folder (Google Drive). 
     *
     *  OBS: Remember to destroy objects when leaving the scene.
     */



    //Variables that should  be set when selecting level (Names of the level, material and skybox).
    public static string LoadedLevel;
    public static string LoadedMaterial1;
    public static string LoadedSkybox;

    //Variables to store level objects, material and skybox. The script uses the strings above and finds the right objects/materials in
    //"Resource" folder in the Unity project (create one if you dont already have one).
    public GameObject LevelComponents;
    public Material Material1;
    public Material Skybox;

    int NrOfChildren;

    void Start()
    {
        ///////// LEVEL COMPONENTS ////////
        LevelComponents = (GameObject)Instantiate(Resources.Load(LoadedLevel));
        NrOfChildren = LevelComponents.transform.childCount;

        Material1 = (Material)Resources.Load(LoadedMaterial1, typeof(Material));

        //If material is given, add the material to all the level components 
        if (Material1)
        {
            for (int i = 0; i < NrOfChildren; i++)
            {
                LevelComponents.transform.GetChild(i).GetComponent<Renderer>().material = Material1;
            }
        }
        
        ///////// SKYBOX ////////
        Skybox = (Material)Resources.Load(LoadedSkybox, typeof(Material));
        RenderSettings.skybox = Skybox;


        //Uncomment this if you want to keep the level in the scene when leaving it. Else it will be destroyed automatically.
        /* 
        DontDestroyOnLoad(LevelComponents);
        DontDestroyOnLoad(Material1);
        DontDestroyOnLoad(Skybox);
        */
    }
}