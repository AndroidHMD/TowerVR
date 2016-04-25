using UnityEngine;
using System.Collections;

/// <HOW TO USE THIS SCRIPT>
/// Place the script on the scene manager and it will "fade" the screen automatically when changing scene.
/// Public variable fadeSpeed determines the duration of the fading effect.
/// Public variable fadeOutTexture needs to have an images assigned, for example a black Image.  
/// </summary>


public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.0f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1; //-1: fade in, 1:fade out

    
    void OnGUI()
    {
        //fade out/in depending on fadeDir
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force (clamp) the number between 0 and 1 (GUI colors: between 0 and 1)
        alpha = Mathf.Clamp01(alpha);

		Debug.Log ("Alpha: " + alpha);

        //set color of GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }
    

    //sets fadeDir to the direciton parameter (making the scene fade in/out)
    public float BeginFade(int direction)
    {
		Debug.Log ("Begin fade to: " + direction);
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1); //Call the fade in function
    }
}
