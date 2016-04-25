using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public sealed class RemoteTowerGameManagerImpl : TowerGameManagerImpl, ITowerGameManager
    {       
        #region PRIVATE_MEMBER_FUNCTIONS
        
        private static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
    }
}