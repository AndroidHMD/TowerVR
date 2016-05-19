using UnityEngine;
using System.Collections;

/**
* This script restricts availbility of an object ("this") 
* to game room host (e.g. master according to Photon Unity Networking)
*/
public class RestrictToHost : Photon.MonoBehaviour{
    
    public bool debug = false;
    private MeshRenderer meshRenderer;
    private Collider col;
    
    void Start() 
    {
		meshRenderer = this.GetComponent<MeshRenderer>();
        col = this.GetComponent<Collider>();
    }
    
    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // if (debug)
            //     Debug.Log("defined, mine");
            
            meshRenderer.enabled = true;
            col.enabled = true;
        } 
        
        else 
        {
            // if (debug)
            //     Debug.Log("not mine");
            
            meshRenderer.enabled = false;
            col.enabled = false;
        }
    }
}