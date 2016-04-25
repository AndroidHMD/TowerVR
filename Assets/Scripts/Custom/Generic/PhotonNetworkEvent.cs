using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

/**
 * Abstract base class for events sent using the Photon API.
 **/
public abstract class PhotonNetworkEvent
{
    protected byte eventCode { get; set; }
    protected object content { get; set; }
    protected RaiseEventOptions eventOptions { get; set; }
    
    public PhotonNetworkEvent()
    {
        eventCode = 200;
        content = null;
        eventOptions = RaiseEventOptions.Default;
    }
    
    protected void setReceivers(ReceiverGroup receiverGroup)
    {
        eventOptions.TargetActors = new int[0];
        eventOptions.Receivers = receiverGroup;
    }
    
    protected void setReceivers(params int[] playerIDs)
    {
        eventOptions.TargetActors = playerIDs;
    }
    
    public bool trySend()
    {
        if (eventCode < 0 || eventCode > 199)
        {
            return false;
        }

        return PhotonNetwork.RaiseEvent(
            eventCode,
            content,
            true,
            eventOptions
        );
    }
}