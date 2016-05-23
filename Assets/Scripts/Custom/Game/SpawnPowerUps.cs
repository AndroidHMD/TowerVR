using UnityEngine;
using System.Collections;

public class SpawnPowerUps : MonoBehaviour {

	public float spawnTime;
	public string prefabName;
	Vector3 randomPosition;

	float time = 0;

	void Update () {
	
		time += Time.deltaTime;

		if (time > spawnTime) {

			randomPosition.x = Random.Range(-200, 200);
			randomPosition.y = Random.Range(50, 100);
			randomPosition.z = Random.Range(-200, 200);

			GameObject powerUp = PhotonNetwork.Instantiate (prefabName, randomPosition, Quaternion.identity, 0) as GameObject;

			time = 0;
		}
	}
}
