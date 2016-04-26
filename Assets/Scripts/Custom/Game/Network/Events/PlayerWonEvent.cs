using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when one of the players won the game.
     **/
    public sealed class PlayerWonEvent : PhotonNetworkEvent
    {
        public PlayerWonEvent(int playerID)
        {
            eventCode = NetworkEventCodes.PlayerWon;
            setReceivers(ReceiverGroup.All);
            
            content = playerID;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out int playerID)
        {
            if (obj is int)
            {
                playerID = (int) obj;
                return true;
            }
            
            playerID = -1;
            return false;
        }
    }
}