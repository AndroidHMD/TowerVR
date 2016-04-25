using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * Event that alerts the master client and tries to start the game.
     **/
    public sealed class TryStartGameEvent : PhotonNetworkEvent
    {
        public TryStartGameEvent()
        {
            eventCode = NetworkEventCodes.TryStartGame;
            setReceivers(ReceiverGroup.MasterClient);   
        }
    } 
}