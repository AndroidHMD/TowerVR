using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpawnSelectedLevel : Photon.PunBehaviour
{

    /** Assume the level selection scene is "Scene 0" and the game scene is "Scene 1".
     *
     *  Add this script to the scene manager in the game scene. (TowerStacker - index 3)
     *  This scripts needs info from another script on the level-selection object ( in scene Menu-ChooseBackdrop - index 1).
     *  The public static string variables in this scripts should be assigned when clicking the level-selection object (in ChooseBackdrop).
     *      -EX: OnClick(){SpawnSelectedLevel.LoadedLevel = LevelName;} //When clicking (selecting the level), set the name of the prefab level to load.
     *
     *  See example in the script "MouseOver.cs" in the Unity/Scripts folder (Google Drive). 
     */

    //Variables that should  be set when selecting level (Names of the level, material and skybox).
    public static string LoadedLevel;
    public static string LoadedSkybox;

    //Variables to store level objects, material and skybox. The script uses the strings above and finds the right objects/materials in
    //"Resource" folder in the Unity project (create one if you dont already have one).

    public GameObject levelObjects;  // stores the prefab that holds all level objects.

    public Material[] towerPieceMaterials;  //Holds materials for tower pieces (0, 1, 2) = (easyMat, mediumMat, hardMat) for a specified level

    public Material Skybox;

    //Name the materials for the tower pieces as "LevelName" + "Mat" + difficulty. EX: MoonEasyMat.
    //Then put the material in the Assets/Resource folder
    private string[] difficulty = {"Easy", "Medium", "Hard" };

    void Start()
    {
        //Set the tower piece materials. 
        //The materials should be named for example "MoonEasyMat" (LevelName + difficulty + "Mat") and be placed in the Resources folder
        
        /*
        for (int i = 0; i < difficulty.Length; i++)
        {
            towerPieceMaterials[i] = (Material)Resources.Load(LoadedLevel + difficulty[i] + "Mat", typeof(Material));
        }
        */

        //Instantiate the level objects prefab.
        if(PhotonNetwork.isMasterClient)
        {
            levelObjects = PhotonNetwork.Instantiate(LoadedLevel, Vector3.down*100, Quaternion.identity, 0) as GameObject;

            //Call Load
            LoadSkybox(LoadedSkybox); //For master
            photonView.RPC("LoadSkybox", PhotonTargets.All, LoadedSkybox); //For everybody else

            photonView.RPC("SetLoadedLevel", PhotonTargets.All, LoadedLevel);
            Debug.Log("Sending materials");
        }

    }

    [PunRPC]
    void LoadSkybox(string skyboxname)
    {
        //Load the skybox and activate it
        Skybox = (Material)Resources.Load(skyboxname, typeof(Material));
        RenderSettings.skybox = Skybox;
    }

    [PunRPC]
    void SetLoadedLevel(string masterLoadedLevel)
    {
        LoadedLevel = masterLoadedLevel;
    }
}