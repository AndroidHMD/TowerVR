using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour {

	public float lifetime;

	void Awake(){

		Destroy (gameObject, lifetime);
	
	}

	void HitPowerUp(){

		GameObject.Find ("GameObject").GetComponent<Shoot> ().HitPowerUp ();

	}
}
