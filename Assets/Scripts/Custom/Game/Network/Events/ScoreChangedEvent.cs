using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event raised when the game score was updated for a player.
     **/
    public sealed class ScoreChangedEvent : PhotonNetworkEvent
    {
        ScoreChangedEvent(int playerID, Score newScore)
        {
            eventCode = NetworkEventCodes.ScoreChanged;
            setReceivers(ReceiverGroup.All);
            
            content = new int[2]
            {
                playerID, newScore.score
            };
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out int playerID, out Score newScore)
        {
            int[] arr = obj as int[];
            if (arr == null)
            {
                playerID = -1;
                newScore = new Score(-1);
                return false;
            }
            
            playerID = arr[0];
            newScore = new Score(arr[1]);
            
            return true;
        }
    }
}