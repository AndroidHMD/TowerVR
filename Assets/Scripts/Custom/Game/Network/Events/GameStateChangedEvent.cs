using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when the master client has changed the game state.
     **/
    public sealed class GameStateChangedEvent : PhotonNetworkEvent
    {
        GameStateChangedEvent(int validGameState)
        {
            eventCode = NetworkEventCodes.GameStateChanged;
            setReceivers(ReceiverGroup.All);
            
            content = validGameState;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out int newGameState)
        {
            return Int32.TryParse(obj as string, out newGameState);
        }
    }
}