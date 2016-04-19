/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GazeRay : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    public ViewTrigger[] viewTriggers;
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        // Check if the Head gaze direction is intersecting any of the ViewTriggers
        RaycastHit hit;
        Ray cameraGaze = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
        foreach (var trigger in viewTriggers)
        {
            trigger.Focused = hit.collider && (hit.collider.gameObject == trigger.gameObject);
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS
}

