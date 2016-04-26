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
        /**
         * Call this to notify that this client is ready to start the game.
         **/
        void notifyIsReady();
        
        /**
         * Tries to start the game. Will fail silently. If it succeeds the client will be alerted
         * through its GameStateChangedHandler.
         **/
        void tryStartGame();
        
        /**
         * Call this to place a tower piece. Will fail silently if it is not the client's turn.
         **/
        void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY);
    }
}