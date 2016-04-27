using UnityEngine;
using System.Collections;

/**
 * Base Behaviour class that only runs on the PhotonNetwork master client.
 * */
public class MasterClientOnlyBehaviour : MonoBehaviour
{
	void Awake()
	{
        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(this);
        }
	}
}