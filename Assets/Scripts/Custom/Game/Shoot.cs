using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public int speed;
	public Transform transform;
	public string prefabNameNormal;
	public string prefabNamePowerUp;
	bool gotPowerUp = false;
	float powerUpTime = 10;

	void Update(){

		if(Cardboard.SDK.Triggered && gotPowerUp){
				
			GameObject clone = PhotonNetwork.Instantiate (prefabNamePowerUp, transform.position, transform.rotation, 0) as GameObject;
			clone.GetComponent<Rigidbody>().AddForce(transform.forward * speed);


		}
		else if(Cardboard.SDK.Triggered){

			GameObject clone = PhotonNetwork.Instantiate (prefabNameNormal, transform.position, transform.rotation, 0) as GameObject;
			clone.GetComponent<Rigidbody>().AddForce(transform.forward * speed);

		}

		if (gotPowerUp) {
		
			powerUpTime -= Time.deltaTime;

			if (powerUpTime < 0) {
				
				gotPowerUp = false;
				powerUpTime = 10;
			
			}
		}
	}

	public void HitPowerUp(){
	
		gotPowerUp = true;
	}
}