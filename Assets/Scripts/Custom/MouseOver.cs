using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {

    public ChangeScene someScript;
    Color objColor; // =? Color of the Object
    public int LevelIndex;


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            ChangeScene.ChangeToScene(LevelIndex);
        }
    }


    void OnMouseEnter()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }


    void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        //gameObject.GetComponent<Renderer>().material.color = Color.objColor;  //Set to original color
    }
}
