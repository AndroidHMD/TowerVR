using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when a player lost the game.
     **/
    public sealed class PlayerLostEvent : PhotonNetworkEvent
    {
        PlayerLostEvent(int playerID)
        {
            eventCode = NetworkEventCodes.PlayerLost;
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