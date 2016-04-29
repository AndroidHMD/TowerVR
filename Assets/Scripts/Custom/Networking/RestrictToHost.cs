using UnityEngine;
using System.Collections;

/**
* This script restricts availbility of an object ("this") 
* to game room host (e.g. master according to Photon Unity Networking)
*/
public class RestrictToHost : Photon.MonoBehaviour{
    
    public bool debug = false;
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
            if (debug)
                Debug.Log("defined, mine");
            
            meshRenderer.enabled = true;
        } 
        
        else 
        {
            if (debug)
                Debug.Log("not mine");
            
            meshRenderer.enabled = false;
        }
    }
}