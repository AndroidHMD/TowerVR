/* Reads the text of the pressed button and tries to 
 * join a room with that name and then loads 
 * the next scene.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JoinServer : MonoBehaviour {

	public TextMesh text;
	public int LevelIndex;

	public void joinServer(){
		PhotonNetwork.JoinRoom (text.text);
		SceneManager.LoadScene (LevelIndex);
	}
}