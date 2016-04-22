using UnityEngine;
using System.Collections;

public abstract class PhotonNetworkEvent
{
    protected int eventCode { get; set; }
    protected object content { get; set; }
    protected RaiseEventOptions eventOptions { get; set; }
    
    public PhotonNetworkEvent()
    {
        eventCode = -1;
        content = null;
        eventOptions = RaiseEventOptions.Default;
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