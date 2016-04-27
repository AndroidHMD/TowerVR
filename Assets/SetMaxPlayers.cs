using UnityEngine;
using System.Collections;

public class SetMaxPlayers : MonoBehaviour {

	public static int maxPlayers = 0;
	public int players;
	Color color;

	void Start(){
	
		color = gameObject.GetComponent<Renderer> ().material.color;
	
	}

	void Update(){

		if (maxPlayers == players)
			gameObject.GetComponent<Renderer> ().material.color = Color.green;
		else
			gameObject.GetComponent<Renderer> ().material.color = color;
	}

	public void setActive(){
	
		maxPlayers = players;
	
	}
}
