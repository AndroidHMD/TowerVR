﻿using UnityEngine;
using System.Collections;

/**
 *	Detects if a towerpiece has fallen down. Will notify ObserveTower in MasterTowerGameManager
 *
 **/

namespace TowerVR
{
	public class FallingTowerDetection : TowerVR.TowerGameBehaviour  {
		public static int nDetectedColliders = 0;
		private bool firstHasEntered;
		
		void Start () 
		{
			//Don't render the plane
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			firstHasEntered = false;
		}
		
		void OnTriggerEnter(Collider collider)
		{
			Debug.Log("Collider Layer: " + collider.gameObject.layer + "GameObject: " + collider.gameObject);
			if(collider.gameObject.layer != 9 && nDetectedColliders < 1)
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
}