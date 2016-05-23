using UnityEngine;
using System.Collections;

public class PlayerNameChooser : MonoBehaviour {
	public Color normalColor;
	public Color onHoverColor;
	
	public TextMesh textMesh;

	void Start () {
		GenerateAndSetPlayerName();
		textMesh.color = normalColor;
	}
	
	private void GenerateAndSetPlayerName()
	{	
		string name = PlayerNameGenerator.GenerateName();
		textMesh.text = name;
		PhotonNetwork.playerName = name;
		
		var textBounds = GetComponent<Renderer>().bounds;
		
		var collider = GetComponent<BoxCollider>();
		
		var size = collider.size;
		size.x = 2 * textBounds.extents.x;
		size.y = 2 * textBounds.extents.y;
		collider.size = size;
	}
	
	public void onPointerEnter()
	{
		textMesh.color = onHoverColor;
	}
	
	public void onPointerExit()
	{
		textMesh.color = normalColor;
	}
	
	public void onPointerClick()
	{
		GenerateAndSetPlayerName();
	}
}
