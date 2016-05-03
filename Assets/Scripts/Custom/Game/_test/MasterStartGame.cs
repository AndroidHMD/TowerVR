using UnityEngine;
using System.Collections;

namespace TowerVR
{
    public class MasterStartGame : MasterClientOnlyBehaviour
    {
        private bool canStartGame = false;
        
        void onGameStateChanged(int gameState)
        {
            Log("onGameStateChanged [gameState=" + gameState + "]");
            
            canStartGame = true;
        }
        
        void Start()
        {
            manager.gameStateChangedHandlers.Add(onGameStateChanged);
        }
        
        void Update()
        {
            if (canStartGame && Input.anyKeyDown)
            {
                Log("Start game");
            }
        }
        
        void OnGUI()
        {
            if (canStartGame)
            {
                GUILayout.Label("You can now start the game!");
            }
        }
        
        private static void Log(object obj)
        {
            Debug.Log("MasterStartGame" + obj.ToString());
        }
    }
}