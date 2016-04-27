using UnityEngine;
using System.Collections;

/**
 * Base Behaviour class that ensures the GameObject has a PhotonView and a PhotonTransformView
 * */
public class NetworkedBehaviour : MonoBehaviour
{
	private static int photonViewIDCounter = 0;

	public static int GetUniquePhotonViewID(){
		return photonViewIDCounter++;
	}

	/**
	 * Attaches a PhotonSynchronizer prefab on awake.
	 * */
	void Awake()
	{
		var photonSynchronizer = Instantiate(Resources.Load("PhotonSynchronizer"), 
			Vector3.zero, Quaternion.identity) as GameObject;
	
		photonSynchronizer.transform.parent = transform;
		photonSynchronizer.GetComponent<PhotonView>().viewID = GetUniquePhotonViewID();
	}
}