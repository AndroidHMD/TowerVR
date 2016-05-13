using UnityEngine;
using System.Collections;

/**
 *	Detects if a towerpiece has fallen down. Will notify ObserveTower in MasterTowerGameManager
 *
 **/

namespace TowerVR
{
	public class FallingTowerDetection : TowerVR.TowerGameBehaviour  {

		public static bool hitDetected;
		public static Collider detectedCollider;
		
		private int hits;

		void Start () 
		{
			//Don't render the plane
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			hits = 0;
			hitDetected = false;
		}
		
					
		void OnTriggerEnter(Collider col) 
		{
			hits++;
			Debug.Log("Detection: " + hits);
			
			if(hits > 1)
			{
				detectedCollider = col;
				hitDetected = true;				
			}
			
		}
			
	}
}