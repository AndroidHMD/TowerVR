using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when the sending player is ready to start the game.
     **/
    public class PlayerReadyEvent : PhotonNetworkEvent
    {
        PlayerReadyEvent(int playerID)
        {
            eventCode = NetworkEventCodes.PlayerReady;
            content = playerID;
            
            setReceivers(ReceiverGroup.MasterClient);
        }
        
        public static bool TryParse(object eventContent, out int playerID)
        {
            return int.TryParse(eventContent as string, out playerID);
        }
    }
}