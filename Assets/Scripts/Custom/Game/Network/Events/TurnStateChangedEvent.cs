using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when the master client has changed the current turn's state.
     **/
    public sealed class TurnStateChangedEvent : PhotonNetworkEvent
    {
        public TurnStateChangedEvent(int validTurnState)
        {
            eventCode = NetworkEventCodes.TurnStateChanged;
            setReceivers(ReceiverGroup.All);
            
            content = validTurnState;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out int newTurnState)
        {
            if (obj is int)
            {
                newTurnState = (int) obj;
                return true;
            }
            
            newTurnState = -1;
            return false;
        }
    }
}