using UnityEngine;
using System.Collections;

/**
 *	Detects if a towerpiece has fallen down. Will notify ObserveTower in MasterTowerGameManager
 *
 **/

namespace TowerVR
{
	public class FallingTowerDetection : TowerVR.TowerGameBehaviour  {
		public static int nDetectedColliders = 0;
		private bool firstHasEntered = false;
		
		void Start () 
		{
			//Don't render the plane
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		
		void OnTriggerEnter(Collider collider)
		{
			if (!firstHasEntered)
			{
				firstHasEntered = true;
				return;
			}
			
			++nDetectedColliders;
		}
	}
}