using UnityEngine;
using System.Collections;

public class WaitingForPlayers : MonoBehaviour 
{
	public TextMesh textMesh;
	public string text = "Waiting for players";
	
	public float dotsPerSecond = 3.0f;
	
	private float startTime = 0.0f;
	
	void Start()
	{
		startTime = Time.time;
	}
	
	void Update () 
	{
		int second = (int) ((Time.time - startTime) * dotsPerSecond);
		
		int nDots = second % 4;
		
		string dots = "";
		for (int i = 0; i < nDots; ++i)
		{
			dots += '.';
		}
		
		textMesh.text = text + dots;
	}
}
