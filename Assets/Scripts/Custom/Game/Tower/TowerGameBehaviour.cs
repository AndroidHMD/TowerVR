using UnityEngine;
using System.Collections;

namespace TowerVR
{
    /**
    * Base Behaviour class that provides convenience access to the scene's TowerGameManager.
    * 
    * Refer to the TowerGameManager's "DELEGATES"-documentation to see usage.
    * */
    public abstract class TowerGameBehaviour : MonoBehaviour
    {
        /**
         * Exposed property that retrieves the scene's TowerGameManager singleton instance.
         **/
        protected TowerGameManager manager 
        {
            get
            {
                return TowerGameManager.Instance;
            }
        }
    }
}