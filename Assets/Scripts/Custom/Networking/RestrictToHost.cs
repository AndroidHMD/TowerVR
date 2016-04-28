using UnityEngine;
using System.Collections;

/**
* This script restricts availbility of an object ("this") 
* to game room host (e.g. master according to Photon Unity Networking)
*/
public class RestrictToHost : Photon.MonoBehaviour{
    private MeshRenderer meshRenderer;
    private PhotonView thisPhotonView;
    
    void Start() 
    {
        thisPhotonView = this.GetComponent<PhotonView>();
		meshRenderer = this.GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        if (thisPhotonView.isMine)
        {
            Debug.Log("defined, mine");
            meshRenderer.enabled = true;
        } 
        
        else 
        {
            Debug.Log("not mine");
            meshRenderer.enabled = false;
        }
    }
}