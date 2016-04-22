using UnityEngine;
using System.Collections;

public class GazeOver : MonoBehaviour {

    private ChangeScene newScene;
    private Color objColor;
    public int LevelIndex;

	public void SwitchScene(){
		ChangeScene.ChangeToScene(LevelIndex);
		Debug.Log ("Change to scene: " + LevelIndex);
	}

	
    public void OnPointerEnter()
    {
		objColor = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }


    public void OnPointerExit()
    {
        gameObject.GetComponent<Renderer>().material.color = objColor;
    }

}
