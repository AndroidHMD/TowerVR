using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerNameChooser : MonoBehaviour {
	public Text text;

	void Start () {
		GenerateAndSetPlayerName();
	}
	
	private void GenerateAndSetPlayerName()
	{
		string name = PlayerNameGenerator.GenerateName();
		text.text = name;
		PhotonNetwork.playerName = name;
	}
}
