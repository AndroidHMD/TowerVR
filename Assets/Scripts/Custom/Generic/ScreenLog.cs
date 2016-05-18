using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenLog : MonoBehaviour {
	private static List<string> entries = new List<string>();
	
	public static void Log(object obj)
	{
		entries.Add(obj.ToString());
	}
	
	void OnGUI()
	{
		foreach (var entry in entries)
		{
			GUILayout.Label(entry);
		}
	}
	
	void Start()
	{
		Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log(""); Log("");
	}
}
