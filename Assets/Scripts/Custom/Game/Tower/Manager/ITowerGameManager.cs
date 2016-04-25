using System;
using System.Collections;
using System.Collections.Generic;

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
    public interface ITowerGameManager
    {
        void notifyIsReady();
        
        void tryStartGame();
    }
}