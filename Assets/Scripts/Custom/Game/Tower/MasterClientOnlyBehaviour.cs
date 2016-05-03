using UnityEngine;
using System.Collections;

namespace TowerVR
{
    /**
     * Base Behaviour class that only runs on the PhotonNetwork master client.
     * */
    public abstract class MasterClientOnlyBehaviour : TowerGameBehaviour
    {
        void Awake()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Destroy(this);
            }
        }
    }
}