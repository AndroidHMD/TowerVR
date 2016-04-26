using UnityEngine;
using System.Collections;

namespace TowerVR
{
    /**
    * Base Behaviour class that provides convenience access to the scene's TowerGameManager
    * */
    public class TowerGameBehaviour : MonoBehaviour
    {
        protected TowerGameManager manager 
        {
            get
            {
                return TowerGameManager.Instance;
            }
        }
    }   
}