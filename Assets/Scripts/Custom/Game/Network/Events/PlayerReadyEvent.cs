using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when the sending player is ready to start the game.
     **/
    public sealed class PlayerReadyEvent : PhotonNetworkEvent
    {
        public PlayerReadyEvent()
        {
            eventCode = NetworkEventCodes.PlayerReady;            
            setReceivers(ReceiverGroup.MasterClient);
        }
    }
}