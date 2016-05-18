using UnityEngine;
using System.Collections;
// using System.Collections.Generic;

/**
 * This script functions as piece selection logic.
 * Pieces are visually changed while looked at.
 * On trigger, set selected bool to true.
 **/

public class SelectingBrick : MonoBehaviour  
{

    public bool selected = false;
    private Color originalColor;

    void Start()
    {
        // selected = false;
        originalColor = gameObject.GetComponent<Renderer>().material.color;
    }
    
    public void OnPointerEnter()
    {
        //Change color to red
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        // Debug.Log("On Enter");
    }

    public void OnPointerExit()
    {
        gameObject.GetComponent<Renderer>().material.color = originalColor;
        // Debug.Log("On Exit");
    }
    
    public void SelectThisBrick()
    {
        selected = true;
        // Debug.Log("Set this brick to true");
    }
}
