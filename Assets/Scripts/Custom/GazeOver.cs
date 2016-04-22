using UnityEngine;
using System.Collections;

public class GazeOver : MonoBehaviour {

    private ChangeScene newScene;
    private Color objColor;
    public int LevelIndex;
	public Cardboard myCardboard;


	public void SwitchScene(){
		gameObject.GetComponent<Renderer>().material.color = Color.red;
		ChangeScene.ChangeToScene(LevelIndex);
		Debug.Log ("Change to scene: " + LevelIndex);
	}


    void OnPointerOver()
    {
		if (myCardboard.Triggered || Input.GetMouseButtonDown(0))
        {
            ChangeScene.ChangeToScene(LevelIndex);
			Debug.Log ("Change to scene: " + LevelIndex);
        }
    }


    void OnPointerEnter()
    {
		objColor = gameObject.GetComponent<Renderer> ().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }


    void OnPointerExit()
    {
        gameObject.GetComponent<Renderer>().material.color = objColor;
    }

}
