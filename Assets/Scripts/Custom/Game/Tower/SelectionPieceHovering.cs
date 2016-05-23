using UnityEngine;
using System.Collections;
// using System.Collections.Generic;

/**
 * This script handles the visual feedback on a tower piece to be selected while hovered on by the corsair.
 **/

public class SelectionPieceHovering : MonoBehaviour  
{

    // private Color originalColor;
    private Behaviour halo;

    void Start()
    {
        halo = (Behaviour)this.GetComponent("Halo");   
        // selected = false;
        // originalColor = gameObject.GetComponent<Renderer>().material.color;
    }
    
    public void OnPointerEnter()
    {
        //Change color to red
        // gameObject.GetComponent<Renderer>().material.color = Color.red;
        // Debug.Log("On Enter");
        halo.enabled = true;
    }

    public void OnPointerExit()
    {
        // gameObject.GetComponent<Renderer>().material.color = originalColor;
        // Debug.Log("On Exit");
        halo.enabled = false;
    }
    
    public void HoveringBehaviour()
    {
        this.transform.RotateAround(this.transform.position, this.transform.up, 2.0f);
    }
    
    public void ConstantBehaviour()
    {
        this.transform.RotateAround(this.transform.position, this.transform.up, 0.3f);
    }
       
}
