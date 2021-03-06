using UnityEngine;
using System.Collections;

/*
 * A class for showing the actual fps. 
 * Add to GameLogic-object in scene 
 */
 
public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
 
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}
 
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
 
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.8f, 1.0f, 1.0f, 1.0f);
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.} fps", fps);
		GUI.Label(rect, text, style);
	}
}
