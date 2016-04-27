using UnityEngine;
using System.Collections;

/*
 * This script will controll how the bricks will be placed.
 * 
 */

/*
 * Tankar: Ha olika material/tyngder på brickorna?
 * 
 * */

public class PlacingBricks : MonoBehaviour {


	// Update is called once per frame
	void Update () {

		//Check if it is my turn, otherwise just observe


		//If it is my turn, spawn brick. The grid should follow my movements now.

		//The brick should be projected onto the tower 


	}
	/*
	public float speed;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
		}
	}
	*/
}
