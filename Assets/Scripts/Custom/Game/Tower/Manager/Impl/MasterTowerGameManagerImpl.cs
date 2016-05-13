using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
    /**
     * An ITowerGameManager that represents the master client's instance. This keeps track of the
     * state as well.
     **/ 
    public sealed class MasterTowerGameManagerImpl : TowerGameManagerImpl
    {      
        
        private List<GameObject> stackedTowerPieces;
        
        #region PUBLIC_MEMBER_FUNCTIONS
		
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
		public sealed override void notifyIsReady()
		{
			handlePlayerReadyEvent(PhotonNetwork.player.ID);
		}
        
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
        public sealed override void tryStartGame()
		{
			handleTryStartGameEvent(PhotonNetwork.player.ID);
		}
        
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
        public sealed override void selectTowerPiece(TowerPieceDifficulty difficulty)
		{
			handleSelectTowerPieceEvent(difficulty);
		}
		
        /**
         * Overrides the event-sending to directly alert this implementation.
         **/
		public sealed override void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY)
		{
			handlePlaceTowerPieceEvent(PhotonNetwork.player.ID, positionX, positionZ, rotationDegreesY);
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
        
        
        
         
        #region PROTECTED_MEMBER_FUNCTIONS
        
        protected sealed override void Awake()
        {	
            base.Awake();
            turnTimer = gameObject.AddComponent<Timer>();
            
			gameState = GameState.AwaitingPlayers;
			turnState = TurnState.NotStarted;
			towerState = TowerState.Stationary;
			
			players = new HashSet<PhotonPlayer>();
			playersReadyMap = new Dictionary<PhotonPlayer, bool>();
			
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				players.Add(photonPlayer);
				playersReadyMap.Add(photonPlayer, false);
			}
        }
        
        protected sealed override void onEvent(byte eventCode, object content, int senderID)
		{
            base.onEvent(eventCode, content, senderID);
            
			switch (eventCode)
			{
				case NetworkEventCodes.PlayerReady:
				{
					handlePlayerReadyEvent(senderID); 
					break;	
				}
				
				case NetworkEventCodes.TryStartGame:
				{
					handleTryStartGameEvent(senderID); 
					break;	
				}
                
                case NetworkEventCodes.SelectTowerPiece:
				{
                    TowerPieceDifficulty difficulty;
                    if (SelectTowerPieceEvent.TryParse(content, out difficulty))
                    {
                        handleSelectTowerPieceEvent(difficulty);
                    }
					else
                    {
                        LogMalformedEventContent("SelectTowerPieceEvent", senderID);
                    }
					break;	
				}
				
				case NetworkEventCodes.PlaceTowerPiece:
				{
					float posX, posZ, rotDegreesY;
                    if (PlaceTowerPieceEvent.TryParse(content, out posX, out posZ, out rotDegreesY))
                    {
                        handlePlaceTowerPieceEvent(senderID, posX, posZ, rotDegreesY);
                    }
                    else
                    {
                        LogMalformedEventContent("PlaceTowerPieceEvent", senderID);
                    }
					break;	
				}
				
				default:
					return;
			}
		}
        
        //////////////////////////////////
		/// PhotonNetworkEvent handles ///
		//////////////////////////////////
        
        protected void handlePlayerReadyEvent(int playerID)
        {
            Log("handlePlayerReadyEvent");
            
            if (gameState != GameState.AwaitingPlayers)
            {
                Error("Player called notifyIsReady when game already running.");
                return;
            }
            
            PhotonPlayer photonPlayer;
            if (tryGetPhotonPlayer(playerID, out photonPlayer))
            {
                playersReadyMap[photonPlayer] = true;
            }
            
            if (allPlayersReady())
            {
                gameState = GameState.AllPlayersReady;
            }
        }
        
		protected void handleTryStartGameEvent(int playerID)
        {
            Log("handleTryStartGameEvent");
            
            if (gameState != GameState.AllPlayersReady)
            {
                return;
            }
            
            if (allPlayersReady())
            {
                initPlayerTurnQueue();
                proceedPlayerTurn();
                
                gameState = GameState.Running;
                turnTimer.start();
            }
        }
        
        protected void handleSelectTowerPieceEvent(TowerPieceDifficulty difficulty)
        {
            // 3 arrays of brick types
            
            // display logic
            
            // select piece
            
            // return piece   
            turnState = TurnState.PlacingTowerPiece;
            Log("handleSelectTowerPieceEvent. Difficulty: " + difficulty);
        }
        
		protected void handlePlaceTowerPieceEvent(int playerID, float posX, float posZ, float rotDegreesY)
        {
            stackedTowerPieces = new List<GameObject>(); //Cleans old list
            
			//Player will try and place TowerPiece in PlacingBrick.cs    
            GameObject[] newPieces = GameObject.FindGameObjectsWithTag("newTowerPiece");
            
            int numberOfObjects = 0;
            foreach (GameObject towerPiece in newPieces)
            {
                numberOfObjects++;
                stackedTowerPieces.Add(towerPiece);
                
                /*var rb = towerPiece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = true;
                    rb.useGravity = true;
                }*/
                
                // Take over ownership
                var photonView = towerPiece.GetComponent<PhotonView>();
                if (photonView != null && !photonView.isMine)
                {
                    photonView.RequestOwnership();
                }
            }
            
            Log("Stacked: " + numberOfObjects + " objects");

            Log("handlePlaceTowerPieceEvent [playerID=" + playerID + " posX=" + posX + " posZ=" + posZ + "rotDegreesY=" + rotDegreesY + "]");
            
            //Update towerState
            turnState = TurnState.TowerReacting;
            
            //Update TowerState
            towerState = TowerState.Moving;
            StartCoroutine(observeTower());
            
        }
        
        
        #endregion PROTECTED_MEMBER_FUNCTIONS
        
        
        
        #region PRIVATE_MEMBER_FUNCTIONS
        
        IEnumerator updateGameState()
        {
            for (;;)
            {
                //Log("updateGameState");
                
                yield return new WaitForSeconds(ONE_SECOND);
            }
        }
        
        IEnumerator updateTurnState()
        {
            for (;;)
            {
                if (gameState == GameState.Running)
                {
                    switch (turnState)
                    {
                        case TurnState.SelectingTowerPiece:
                            if (turnTimer.time > TurnTimeLimits.SelectingTowerPiece)
                            {
                                turnState = TurnState.PlacingTowerPiece;
                            }
                            break;
                        case TurnState.PlacingTowerPiece:
                            if (turnTimer.time > TurnTimeLimits.PlacingTowerPiece)
                            {
                                turnState = TurnState.TowerReacting;
                            }
                            break;
                        case TurnState.TowerReacting:
                            
                            if (towerState == TowerState.Stationary)
                            {
                                proceedPlayerTurn();
                                turnState = TurnState.SelectingTowerPiece;
                            }
                            break;
                    }
                }
                
                yield return new WaitForSeconds(ONE_SECOND);
            }
        }
        
        IEnumerator observeTower()
        {
            yield return new WaitForSeconds(TurnTimeLimits.TowerReacting);
            
            
            while(towerState == TowerState.Moving)
            {
                bool allPiecesStationary = true;
                foreach (var towerPiece in stackedTowerPieces)
                {
                    var rb = towerPiece.GetComponent<Rigidbody>();
                    if(rb.velocity.magnitude > TowerConstants.MaxTowerVelocity || rb.angularVelocity.magnitude > TowerConstants.MaxTowerAngVelocity)
                    {
                        allPiecesStationary = false;
                        break;
                    }
                    
                }
                if(allPiecesStationary)
                {
                    towerState = TowerState.Stationary;
                    StopCoroutine(observeTower());
                }
                yield return null;
            }
            
        }

        
        void Start()
        {
            StartCoroutine("updateGameState");
            StartCoroutine("updateTurnState");
        }
        
        void OnDestroy()
        {
            StopCoroutine("updateGameState");
            StopCoroutine("updateTurnState");
        }
        
        /**
         * Initializes the player turn queue with the players that were online on scene startup.
         **/
        private void initPlayerTurnQueue()
        {
            playerQueue = new Queue<PhotonPlayer>();
            
            foreach (var photonPlayer in players)
            {
                playerQueue.Enqueue(photonPlayer);
            }
        }
        
        /**
         * Ends the current player's turn and continues to the next player.
         **/
        private void proceedPlayerTurn()
        {
            PhotonPlayer lastPhotonPlayer = currentPlayer;
            currentPlayer = null;
            
            if (lastPhotonPlayer != null)
            {
                playerQueue.Enqueue(lastPhotonPlayer);
            }
            
            var nextPlayer = playerQueue.Dequeue();
            
            var ev = new NextPlayerEvent(nextPlayer.ID);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
                gameState = GameState.Stopped;
                return;
            }
            
            currentPlayer = nextPlayer;
            
            turnState = TurnState.SelectingTowerPiece;
        }
        
        /**
		 * Checks if all players are ready to start the game.
		 * 
		 * Returns true if #reportIsReady has been called with each PhotonPlayer as argument.
		 * */ 
		private bool allPlayersReady()
		{
			foreach (var entry in playersReadyMap)
			{
				if (entry.Value == false)
				{
					return false;
				}
			}
			
			return true;
		}
        
        #endregion PRIVATE_MEMBER_FUNCTIONS
        
        
        #region PRIVATE_MEMBER_VARIABLES
        
        // The game state.
        private int _backingGameState;
		private int gameState
        {
            set
            {
                if (GameState.IsValid(value))
                {
                    // Set state and notify all clients of the new state
                    _backingGameState = value;
                    var ev = new GameStateChangedEvent(_backingGameState);
                    if (!ev.trySend())
                    {
                        Error(ev.trySendError);
                    }
                }
            }
            get { return _backingGameState; }
        }
        
        // The turn state.
        private int _backingTurnState;
		private int turnState
        {
            set
            {
                if (TurnState.IsValid(value))
                {
                    turnTimer.clear();
                    turnTimer.start();
                    
                    // Set state and notify all clients of the new state
                    _backingTurnState = value;   
                    var ev = new TurnStateChangedEvent(_backingTurnState);
                    if (!ev.trySend())
                    {
                        Error(ev.trySendError);
                    }
                }
            }
            get { return _backingTurnState; }
        }
        
        private int towerState;
        
        // The players that we're in the room when the manager was instantiated.
        private HashSet<PhotonPlayer> players;
        
        // Flags to see if each player is online and ready.
        private IDictionary<PhotonPlayer, bool> playersReadyMap;
        
        /**
		 * A queue containing the next players.
		 * 
		 * When the current player's turn is done, the next player is taken from the front of the queue.
		 * 
		 * If the current player performs an action that succeeds, the player is appended to the queue again.
		 * If the current player performs an action that causes the tower to fall, 
		 * 		the player is not appended to the queue again.
		 * */
        private Queue<PhotonPlayer> playerQueue;
        
        // Reference to the current player.
        private PhotonPlayer currentPlayer;
        
        private Timer turnTimer;
        
        #endregion PRIVATE_MEMBER_VARIABLES
        
        /**
         * Helper to retrieve a PhotonPlayer instance based on an ID.
         **/
        private bool tryGetPhotonPlayer(int playerID, out PhotonPlayer photonPlayer)
        {
            foreach (var _photonPlayer in players)
            {
                if (_photonPlayer.ID == playerID)
                {
                    photonPlayer = _photonPlayer;
                    return true;
                }
            }
            
            photonPlayer = null;
            return false;
        }
        
        private static void Log(object obj)
        {
            Debug.Log("MasterTowerGameManagerImpl: " + obj.ToString());
        }
        
        private static void Error(object obj)
        {
            Debug.LogError("MasterTowerGameManagerImpl: " + obj.ToString());
        }
        
        private const float ONE_TENTH_SECOND = 0.1f;
        private const float ONE_SECOND = 1.0f;
    }
}