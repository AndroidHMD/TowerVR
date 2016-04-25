using UnityEngine;
using System.Collections;


public class HostGame : MonoBehaviour  {

	public int LevelIndex;

	public void Host(){
		ChangeScene.ChangeToScene (LevelIndex);
	}
}