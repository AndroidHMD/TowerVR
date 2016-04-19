/*============================================================================== 
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/
using UnityEngine;
using System.Collections;
using Vuforia;

public class ViewModeManager : MonoBehaviour {

    #region PUBLIC_MEMBER_VARIABLES

    public GameObject stereoDivision;

    #endregion


    #region MONOBEHAVIOUR_METHODS

    void Awake () 
    {
        VuforiaAbstractBehaviour vuforia = FindObjectOfType<VuforiaAbstractBehaviour>();
        vuforia.RegisterVuforiaInitializedCallback(SetViewMode);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS
    internal void SetViewMode()
    {
		if (ModeConfig.isFullScreenMode) {
			MixedRealityController.Instance.SetMode (MixedRealityController.Mode.HANDHELD_AR);
        } else
        {
            // Deactivate the alignment bar used only for stereo mode
			stereoDivision.SetActive(true);
        }
    }

    #endregion // PRIVATE_METHODS

}
