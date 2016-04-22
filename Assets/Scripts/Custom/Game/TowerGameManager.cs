using System;

namespace TowerVR
{
    /**
     * Interface to the class that controls the game logic.
     * 
     * The class handles:
	 * - Game state: is the game running? Has it ended?
	 * - Turn state: what action is the current player doing?
	 * - Turn order: In what order does the players play? Has anyone lost yet?
	 * - Scores
     * 
     * The implementing class is either a local instance (if the current client is the
     * master client), or a remote instance (if the current client is NOT the master client).
     * */
    public interface TowerGameManager
    {
        void getGameState(Action<GameState> callback);
        
        void getTurnState(Action<TurnState> callback);
        
        void notifyIsReady(int playerID);
        
        void tryStartGame(Action<bool> callback);
        
        void tryGetScore(int playerID, Action<Score> callback);
    }
}