using UnityEngine;
using System.Collections;

public class SetMaxPlayers : MonoBehaviour {

	// Currently chosen max players
	public static int maxPlayers = 4;
	// Number of players for this cube
	public int players;
	// color of the object
	Color color;


	void Start(){
		// gets the original color of the object
		color = gameObject.GetComponent<Renderer> ().material.color;
	
	}

	// Check if this is the active object, if so change it's color to green
	// else give it its original color
	void Update(){

		if (maxPlayers == players)
		{
			gameObject.GetComponent<Renderer> ().material.color = Color.green;
		}
		else
		{
			gameObject.GetComponent<Renderer> ().material.color = color;
		}
			
	}

	// sets this object to the currently active one.
	public void setActive(){
	
		maxPlayers = players;
	
	}
}
