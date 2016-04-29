using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

namespace TowerVR
{
    /**
     * An event sent to the master client when the current player places his/her current tower piece.
     **/
    public sealed class SelectTowerPieceEvent : PhotonNetworkEvent
    {
        public SelectTowerPieceEvent(TowerPieceDifficulty difficulty)
        {
            eventCode = NetworkEventCodes.NextPlayer;
            setReceivers(ReceiverGroup.MasterClient);
            
            switch (difficulty)
            {
                case TowerPieceDifficulty.Easy:
                    content = 0; break;
                case TowerPieceDifficulty.Medium:
                    content = 1; break;
                case TowerPieceDifficulty.Hard:
                    content = 2; break;
                default:
                    content = -1; break;
            }
        }
        
        protected sealed override bool contentIsValid()
        {
            int diff;
            
            if (!Int32.TryParse(content as string, out diff))
            { return false; }
            
            if (diff != 0 || diff != 1 || diff != 2)
            {
                trySendError = "Difficulty must be Easy/Medium/Hard.";
                return false;
            }
            
            return true;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, TowerPieceDifficulty chosenDifficulty)
        {
            int diff;
            chosenDifficulty = TowerPieceDifficulty.Easy;
            
            if (!Int32.TryParse(obj as string, out diff))
            { return false; }
            
            switch (diff)
            {
                case 0:
                    chosenDifficulty = TowerPieceDifficulty.Easy; break;
                case 1:
                    chosenDifficulty = TowerPieceDifficulty.Medium; break;
                case 2:
                    chosenDifficulty = TowerPieceDifficulty.Hard; break;
                default:
                    return false;
            }
            
            return true;
        }
    }
}