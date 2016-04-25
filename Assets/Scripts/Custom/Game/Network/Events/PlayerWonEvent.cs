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