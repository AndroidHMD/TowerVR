using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    public sealed class RemoteTowerGameManagerImpl : TowerGameManagerImpl, ITowerGameManager
    {       
        #region PROTECTED_MEMBER_FUNCTIONS
         
        protected override void _onEventHandler(byte eventCode, object content, int senderId)
        {
            
        }
         
        protected override void _notifyIsReady(int playerID)
        {
            Log("notifyIsReady [playerID=" + playerID + "]");
            
            //todo
        }
        
        protected override void _tryStartGame()
        {
            Log("tryStartGame");
            
            //todo
        }
        
        #endregion PROTECTED_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
        
        private static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
        
        
        
        
        #region PRIVATE_MEMBER_VARIABLES

        
        #endregion PRIVATE_MEMBER_VARIABLES
    }
}