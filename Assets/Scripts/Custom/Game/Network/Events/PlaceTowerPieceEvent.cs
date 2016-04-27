using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event sent to the master client when the current player places his/her current tower piece.
     **/
    public sealed class PlaceTowerPieceEvent : PhotonNetworkEvent
    {
        public PlaceTowerPieceEvent(float positionX,
                                    float positionZ,
                                    float rotationDegreesY)
        {
            eventCode = NetworkEventCodes.NextPlayer;
            setReceivers(ReceiverGroup.MasterClient);
            
            content = new float[3]
            {
                positionX,
                positionZ,
                rotationDegreesY
            };
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out float posX, out float posZ, out float rotDegreesY)
        {
            var arr = obj as float[];
            if (arr == null || arr.Length != 3)
            {
                posX = posZ = rotDegreesY = 0.0f;
                return false;
            }
            
            posX = arr[0];
            posZ = arr[1];
            rotDegreesY = arr[2];
            
            return true;
        }
    }
}