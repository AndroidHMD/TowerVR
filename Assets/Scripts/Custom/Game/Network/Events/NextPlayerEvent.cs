using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when a new player's turn is about to start.
     **/
    public sealed class NextPlayerEvent : PhotonNetworkEvent
    {
        public NextPlayerEvent(int playerID)
        {
            eventCode = NetworkEventCodes.NextPlayer;
            setReceivers(ReceiverGroup.All);
            
            content = playerID;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out int playerID)
        {
            return Int32.TryParse(obj as string, out playerID);
        }
    }
}