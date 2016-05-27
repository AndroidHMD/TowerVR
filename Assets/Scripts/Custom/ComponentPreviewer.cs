using UnityEngine;
using System.Collections;

/// <This script fades a singel GameObject: Used for previewing a level.>
/// 
/// YOU DO NOT NEED TO ATTACH THIS SCRIPT MANUALLY:
/// This script needs to be attached to all level components (children of an empty gameObject) that should be previewed. This is done with the loop marked with (x).
/// The loop (x) should be in the Start()-function of the miniature level object (which is also the trigger object). This should now be placed in the GazeOverPreview script.
/// 
/// The miniature should also have a bool: "public bool isGazedOn" since this script is accessing it. 
/// isGazedOn should be set to true only if he miniature is being gazed on, otherwise it should be false.
/// 
/// </summary>
/// 


//     (x)
/*
for (int childIndex = 0; childIndex<parentObject.transform.childCount; childIndex++)
        {
            Transform child = parentObject.transform.GetChild(childIndex);

            child.gameObject.AddComponent<ComponentPreviewer>();
            child.gameObject.GetComponent<ComponentPreviewer>().triggerObject = gameObject;
        }
*/




public class ComponentPreviewer : MonoBehaviour {

    //Holds the RGB values
    private float currRed;
    private float currGreen;
    private float currBlue;

    //Alpha determines the transparacy level. (0 is transparant and 1 is opaque). 
    private float alpha = 0f;

    //fadeFactor determines the fade speed. Lower fadeFactor -> Longer fade Time.
    private float fadeFactor = 0.8f;

    //triggerObject is the object which sets the bool and triggers the fade effect (The Gazed-on object).
    public GameObject triggerObject;

    //Holds the dimension sizes (x, y, z) of the current object.
    private Vector3 objectSize;


    void Start()
    {
        toggleLight(false);
        
        objectSize = gameObject.transform.localScale;

        currRed = gameObject.GetComponent<Renderer>().material.color.r;
        currGreen = gameObject.GetComponent<Renderer>().material.color.g;
        currBlue = gameObject.GetComponent<Renderer>().material.color.b;
    }


    void Update()
    {
        //The component GazedOverPreview is the script where "isGazedOn" is located. (The GazedOn script). 
        if (triggerObject.GetComponent<GazeOverPreview>().isGazedOn)
        {
            //SetActive() does not work when using on all the children for some reason.
            //Instead the object is scaled down to 0 in all dimensions when it should not be visible, and scaled to its original size when visible.
            gameObject.transform.localScale = new Vector3(objectSize.x, objectSize.y, objectSize.z);

            if (alpha <= 1)
                fadeIn();
        }
        else
        {
            if (alpha > 0)
            {
                fadeOut();
            }

            if (alpha <= 0)
            {
                //gameObject.SetActive(false);

                //SetActive() does not work when using on all the children for some reason.
                //Instead the object is scaled down to 0 in all dimensions when it should not be visible, and scaled to its original size when visible.
                gameObject.transform.localScale = new Vector3(0f, 0f, 0f); 
            }
        }
    }
    
    void toggleLight(bool enable)
    {
        var light = gameObject.GetComponent<Light>();
        if (light)
        {
            light.enabled = enable;
        }
    }


    //Fade out a gameObject (will be added on all the children of the level object)
    void fadeOut()
    {
        toggleLight(false);
        
        var renderer = gameObject.GetComponent<Renderer>();
        if (!renderer) return;
        
        renderer.material.color = new Vector4(currRed, currGreen, currBlue, alpha);
        alpha -= Time.deltaTime * fadeFactor;
    }


    //Fade in a gameObject (will be added on all the children of the level object)
    void fadeIn()
    {
        toggleLight(true);
        
        var renderer = gameObject.GetComponent<Renderer>();
        if (!renderer) return;
         
        renderer.material.color = new Vector4(currRed, currGreen, currBlue, alpha);
        alpha += Time.deltaTime * fadeFactor;
    }
}
