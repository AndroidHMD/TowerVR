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
            
            if (!(content is int))
            { 
                Debug.Log("Content could not be read!");
                return false; 
            }
            
            diff = (int) content;
            
            if (diff != 0 && diff != 1 && diff != 2)
            {
               Debug.Log("Difficulty must be Easy/Medium/Hard.");
                return false;
            }
            return true;
        }
        
        /**
         * Tries to parse the contents of the event.
         **/
        public static bool TryParse(object obj, out TowerPieceDifficulty chosenDifficulty)
        {
            int diff;
            chosenDifficulty = TowerPieceDifficulty.Easy;
            
            if (!(obj is int))
            { return false; }
            
            diff = (int) obj;
            
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