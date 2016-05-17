using UnityEngine;
using System.Collections;

public class PlayerColor : Photon.MonoBehaviour {
	private Color color;
	
	void Start()
	{
		color = getPlayerColor();
		color.a = 0.75f;
		
		var systems = gameObject.GetComponentsInChildren<ParticleSystem>();
		var lights = gameObject.GetComponentsInChildren<Light>();
				
		foreach (var system in systems)
		{
			system.startColor = color;
		}
		
		foreach (var light in lights)
		{
			light.color = color;
		}
	}
	
	Color getPlayerColor()
	{
		if (PhotonNetwork.connected && PhotonNetwork.inRoom)
		{
			switch (photonView.ownerId)
			{
				case 1: return Color.red;
				case 2: return Color.green;
				case 3: return Color.blue;
				case 4: return Color.yellow;
				case 5: return Color.cyan;
				case 6: return Color.magenta;
				case 7: return Color.grey;
			}
		}
		
		return Color.red;
	}
}
