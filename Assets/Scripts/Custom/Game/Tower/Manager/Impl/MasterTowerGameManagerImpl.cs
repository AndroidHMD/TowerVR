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
        void OnGUI()
        {
            if (gameState == GameState.Running)
            {
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                GUILayout.Label("");
                
                foreach (var playerScorePair in playerScores)
                {
                    var player = playerScorePair.Key;
                    var score = playerScorePair.Value;
                    GUILayout.Label("PlayerID: " + player.ID + ", score: " + score.score);
                }
            }
        }
        
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
			handleSelectTowerPieceEvent(PhotonNetwork.player.ID, difficulty);
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
            playerScores = new Dictionary<PhotonPlayer, Score>();
			
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				players.Add(photonPlayer);
				
                playersReadyMap.Add(photonPlayer, false);
                playerScores.Add(photonPlayer, new Score(0));
			}
            
            stackedTowerPieces = new List<GameObject>();
            stackedTowerPiecesTransformData = new Dictionary<GameObject, TransformData>();
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
                        handleSelectTowerPieceEvent(senderID, difficulty);
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
                turnState = TurnState.SelectingTowerPiece;
            }
        }
        
        protected void handleSelectTowerPieceEvent(int playerID, TowerPieceDifficulty difficulty)
        {
            currentDifficulty = difficulty;
            turnState = TurnState.PlacingTowerPiece;
            Log("handleSelectTowerPieceEvent. Difficulty: " + difficulty);
        }
        
		protected void handlePlaceTowerPieceEvent(int playerID, float posX, float posZ, float rotDegreesY)
        {
            FallingTowerDetection.nDetectedColliders = 0;
            
            //Player will first try and place TowerPiece in PlacingBrick.cs    
            GameObject[] newPieces = GameObject.FindGameObjectsWithTag("newTowerPiece");
            
            foreach (GameObject towerPiece in newPieces)
            {
                if (stackedTowerPieces.Contains(towerPiece))
                {
                    // piece already in the tower piece list
                    continue;
                }
                                
                // Take over ownership
                var photonView = towerPiece.GetComponent<PhotonView>();
                if (photonView != null && !photonView.isMine)
                {
                    photonView.RequestOwnership();
                    LogToScreen("Requesting ownership of new piece.");
                }
                
                mostRecentTowerPiece = towerPiece;
            }

            Log("handlePlaceTowerPieceEvent [playerID=" + playerID + " posX=" + posX + " posZ=" + posZ + "rotDegreesY=" + rotDegreesY + "]");
            
            //Update towerState
            turnState = TurnState.TowerReacting;
            
            //Update TowerState
            towerState = TowerState.Moving;
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
                Log("updateTurnState");
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
                                towerState = TowerState.Moving;
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
        
        IEnumerator updateTowerState()
        {
            for (;;)
            {
                Log("updateTowerState");
                if(turnState == TurnState.TowerReacting)
                {
                    yield return new WaitForSeconds(TurnTimeLimits.TowerReacting);
                                
                    while(towerState == TowerState.Moving)
                    {
                        if(FallingTowerDetection.nDetectedColliders > 0)
                        {
                            Debug.Log("Tower falling!");
                            
                            handlePlayerLost();
                            
                            var photonView = mostRecentTowerPiece.GetComponent<PhotonView>();
                            if (photonView)
                            {
                                if (!photonView.isMine)
                                {
                                    LogToScreen("Trying to Destroy obj which is NOT mine.");
                                } else {
                                    LogToScreen("Trying to Destroy obj which is mine.");
                                }
                            }
                            
                            PhotonNetwork.Destroy(mostRecentTowerPiece);
                            mostRecentTowerPiece = null;
                            
                            StartCoroutine("restoreTowerTransforms");
                            
                            towerState = TowerState.Stationary;
                            yield return null;
                        }
                        
                        bool allPiecesStationary = true;
                        
                        allPiecesStationary &= checkIsTowerPieceStationary(mostRecentTowerPiece);
                        foreach (var towerPiece in stackedTowerPieces)
                        {
                            allPiecesStationary &= checkIsTowerPieceStationary(towerPiece);
                            if (allPiecesStationary)
                            {
                                break;
                            }
                        }
                        
                        if(allPiecesStationary)
                        {
                            IncreaseHeight.checkIncreaseHeight = true;
                            towerState = TowerState.Stationary;
                            
                            stackedTowerPieces.Add(mostRecentTowerPiece);
                            mostRecentTowerPiece = null;
                            
                            storeTowerTransforms();
                        }
                        yield return null;
                    }
                }
                
                yield return new WaitForSeconds(ONE_SECOND);
            }
        }
        
        bool checkIsTowerPieceStationary(GameObject towerPiece)
        {
            Rigidbody rb = null;
            return (towerPiece != null) &&
                   ((rb = towerPiece.GetComponent<Rigidbody>()) != null) &&
                   (rb.velocity.magnitude < TowerConstants.MaxTowerVelocity || rb.angularVelocity.magnitude < TowerConstants.MaxTowerAngVelocity);
        }
        
        
        void Start()
        {
            StartCoroutine("updateGameState");
            StartCoroutine("updateTurnState");
            StartCoroutine("updateTowerState");
        }
        
        void OnDestroy()
        {
            StopCoroutine("updateGameState");
            StopCoroutine("updateTurnState");
            StopCoroutine("updateTowerState");
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
                
                // update score of last player
                updateScore(lastPhotonPlayer, currentDifficulty);
            }
            
            if (playerQueue.Count == 0)
            {
                endGame();
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
        }
        
        private void updateScore(PhotonPlayer player, TowerPieceDifficulty diff)
        {
            var oldScore = playerScores[player];
            var newScore = Score.Add(oldScore, Score.GetScore(diff));
            
            playerScores[player] = newScore;
            
            var ev = new ScoreChangedEvent(player.ID, newScore);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
                gameState = GameState.Stopped;
            }
        }
        
        private void endGame()
        {
            Log("Game ended!");
            Tuple<PhotonPlayer, Score> winningPlayer = getWinningPlayer();
            
            gameState = GameState.Ended;
            
            var ev = new PlayerWonEvent(winningPlayer.Item1.ID);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
                gameState = GameState.Stopped;
            }
        }
        
        private void handlePlayerLost()
        {
            var losingPlayer = currentPlayer;
            currentPlayer = null;
            
            var ev = new PlayerLostEvent(losingPlayer.ID);
            if (!ev.trySend())
            {
                Error(ev.trySendError);
                gameState = GameState.Stopped;
            }
        }
        
        private IEnumerator restoreTowerTransforms()
        {
            foreach (var towerPiece in stackedTowerPieces)
            {
                TransformData transformData;
                if (!stackedTowerPiecesTransformData.TryGetValue(towerPiece, out transformData))
                {
                    continue;
                }
                
                // Start moving back the pieces
                iTween.MoveTo(towerPiece, transformData.position, TowerConstants.RestoreTowerTime);
                iTween.RotateTo(towerPiece, transformData.rotation.eulerAngles, TowerConstants.RestoreTowerTime);
                
                // Disable all collisions
                var rb = towerPiece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    
                    rb.detectCollisions = false;
                    rb.isKinematic = true;
                }
            }
            
            yield return new WaitForSeconds(TowerConstants.RestoreTowerTime + 0.1f);
            
            foreach (var towerPiece in stackedTowerPieces)
            {
                // Re-enable all collisions
                var rb = towerPiece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.detectCollisions = true;
                    rb.isKinematic = false;
                }
            }
            
            yield return null;
        }
        
        private void storeTowerTransforms()
        {
            foreach (var towerPiece in stackedTowerPieces)
            {
                var transform = towerPiece.transform;
                
                stackedTowerPiecesTransformData[towerPiece] = new TransformData(transform);
            }
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
        
        private Tuple<PhotonPlayer, Score> getWinningPlayer()
        {
            PhotonPlayer winner = null;
            Score score = new Score(-1);
            
            foreach (var playerScorePair in playerScores)
            {
                if (score.score < playerScorePair.Value.score)
                {
                    winner = playerScorePair.Key;
                    score = playerScorePair.Value;
                }
            }
            
            return new Tuple<PhotonPlayer, Score>(winner, score);
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
        
        private struct TransformData
        {
            public readonly Vector3 position;
            public readonly Quaternion rotation;
            
            public TransformData(Transform transform)
            {
                position = transform.position;
                rotation = transform.rotation;
            }
        }
        
        private GameObject mostRecentTowerPiece;
        private List<GameObject> stackedTowerPieces;
        private IDictionary<GameObject, TransformData> stackedTowerPiecesTransformData;
        
        // The players that we're in the room when the manager was instantiated.
        private HashSet<PhotonPlayer> players;
        
        // Flags to see if each player is online and ready.
        private IDictionary<PhotonPlayer, bool> playersReadyMap;
        
        private IDictionary<PhotonPlayer, Score> playerScores;
        
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
        
        private TowerPieceDifficulty currentDifficulty;
        
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
        
        private static void LogToScreen(object obj)
        {
            ScreenLog.Log(obj);
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