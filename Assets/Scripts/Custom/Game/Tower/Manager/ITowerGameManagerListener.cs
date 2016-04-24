using System.Collections.Generic;

namespace TowerVR
{
    public interface ITowerGameManagerListener
    {
        void onGameStateChanged(GameState gameState);
        
        void onTurnStateChanged(TurnState turnState);
        
        void onPlayerLost(PhotonPlayer player);
        
        void onScoreUpdated(IDictionary<PhotonPlayer, Score> scores);
        
        void onPlayerWon(PhotonPlayer player);
    }   
}