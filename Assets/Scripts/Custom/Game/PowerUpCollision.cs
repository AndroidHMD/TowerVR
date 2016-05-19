using UnityEngine;
using System.Collections;

public class PowerUpCollision : MonoBehaviour {

	void OnTriggerEnter(Collider col){

		col.SendMessage ("HitPowerUp");
	
		Destroy (col.gameObject);
		Destroy (gameObject);

	}
}
