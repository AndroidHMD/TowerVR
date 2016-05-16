using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Checks if it's time to increase height of player plattform and performs a change.
 * Runs only on master client.
 **/

namespace TowerVR
{

	public class IncreaseHeight : MasterClientOnlyBehaviour  {


		public static bool checkIncreaseHeight;
		private int towerState;


		void onTowerStateChanged(int towerState)
		{
			this.towerState = towerState;
		}


		void Start ()
		{
			manager.towerStateChangedHandlers.Add (onTowerStateChanged);
			checkIncreaseHeight = false;
		}
	
		
		void OnTriggerStay(Collider col) 
		{
			
			//Only check when new objects has been added and not when placing new objects
			if(checkIncreaseHeight) 
			{
				Debug.Log("Time to increase height!");
				Vector3 newPosition = gameObject.transform.position + Vector3.up * 30; //Multiply with height of collider
				iTween.MoveTo(gameObject, iTween.Hash("position", newPosition, "easetype", "easeInOutSine" ,"time",TowerConstants.IncreaseHeightTime));
				
				checkIncreaseHeight = false;
			}			
		}
		
				
		// Destroy listeners
		void OnDestroy()
		{
			manager.towerStateChangedHandlers.Remove (onTowerStateChanged);
		}

	}
}
