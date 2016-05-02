using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	/**
	 * An ITowerGameManager that represents a remote instance.
	 * 
	 * Do not use this class directly in the game logic. 
	 **/
	public class TowerGameManagerImpl : Singleton<TowerGameManagerImpl>, ITowerGameManager
	{
		#region PUBLIC_MEMBER_FUNCTIONS
		
		/**
		 * Sends an event to the master client implementation.
		 **/
		public virtual void notifyIsReady()
		{
			var ev = new PlayerReadyEvent();
			if (!ev.trySend())
			{
				Debug.LogError(ev.trySendError);
			}
		}
        
		/**
		 * Sends an event to the master client implementation.
		 **/
        public virtual void tryStartGame()
		{
			var ev = new TryStartGameEvent();
			if (!ev.trySend())
			{
				Debug.LogError(ev.trySendError);
			}
		}
		
		/**
		 * Sends an event to the master client implementation.
		 **/
		public virtual void placeTowerPiece(float positionX, float positionZ, float rotationDegreesY)
		{
			var ev = new PlaceTowerPieceEvent(positionX, positionZ, rotationDegreesY);
			if (!ev.trySend())
			{
				Debug.LogError(ev.trySendError);
			}
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		
		
		
		#region PROTECTED_MEMBER_FUNCTIONS
		
		protected virtual void Awake()
		{
			PhotonNetwork.OnEventCall += onEvent;
		}
		
		protected virtual void OnDestroy()
		{
			PhotonNetwork.OnEventCall -= onEvent;
			parent = null;
		}
		
		/**
		 * Receives events from the master client and alerts the observer delegates (i.e. TowerGameBehaviours).
		 **/
		protected virtual void onEvent(byte eventCode, object content, int senderID)
		{
			switch (eventCode)
			{
				case NetworkEventCodes.GameStateChanged:
				{
					int gameState;
					if (GameStateChangedEvent.TryParse(content, out gameState))
					{
						handleGameStateChangedEvent(gameState);
					}
					else
					{
						LogMalformedEventContent("GameStateChangedEvent", senderID);
					}
					break;	
				}
				
				case NetworkEventCodes.TurnStateChanged:
				{
					int turnState;
					if (TurnStateChangedEvent.TryParse(content, out turnState))
					{
						handleTurnStateChangedEvent(turnState);
					}
					else
					{
						LogMalformedEventContent("TurnStateChangedEvent", senderID);
					} 
					break;	
				}
				
				case NetworkEventCodes.NextPlayer:
				{
					int nextPlayerID;
					if (NextPlayerEvent.TryParse(content, out nextPlayerID))
					{
						handleNextPlayerEvent(nextPlayerID);
					}
					else
					{
						LogMalformedEventContent("NextPlayerEvent", senderID);
					} 
					break;	
				}
				
				case NetworkEventCodes.ScoreChanged:
				{
					int playerID;
					Score score;
					if (ScoreChangedEvent.TryParse(content, out playerID, out score))
					{
						handleScoreChangedEvent(playerID, score);
					} 
					else
					{
						LogMalformedEventContent("ScoreChangedEvent", senderID);
					}
					break;	
				}
				
				case NetworkEventCodes.PlayerLost:
				{
					int losingPlayerID;
					if (PlayerLostEvent.TryParse(content, out losingPlayerID))
					{
						handlePlayerLostEvent(losingPlayerID);
					} 
					else
					{
						LogMalformedEventContent("PlayerLostEvent", senderID);
					}
					break;	
				}
				
				case NetworkEventCodes.PlayerWon:
				{
					int winningPlayerID;
					if (PlayerWonEvent.TryParse(content, out winningPlayerID))
					{
						handlePlayerWonEvent(winningPlayerID);
					} 
					else
					{
						LogMalformedEventContent("PlayerLostEvent", senderID);
					} 
					break;	
				}
				
				default:
					return;
			}
		}
		
		#endregion PROTECTED_MEMBER_FUNCTIONS
		
		
		
		#region ABSTRACT_MEMBER_FUNCTIONS
		
		protected virtual void handleGameStateChangedEvent(int gameState) 
		{
			Log("handleGameStateChangedEvent");
			
			foreach (var handler in parent.gameStateChangedHandlers)
			{ handler(gameState); }
		}
		
		protected virtual void handleTurnStateChangedEvent(int turnState) 
		{
			Log("handleTurnStateChangedEvent");
			
			foreach (var handler in parent.turnStateChangedHandlers)
			{ handler(turnState); }
		}
		
		protected virtual void handleNextPlayerEvent(int nextPlayerID) 
		{
			Log("handleNextPlayerEvent");
			
			foreach (var handler in parent.nextPlayerTurnHandlers)
			{ handler(nextPlayerID); }
		}
		
		protected virtual void handleScoreChangedEvent(int playerID, Score score) 
		{
			Log("handleScoreChangedEvent");
			
			foreach (var handler in parent.scoreUpdatedHandlers)
			{ handler(playerID, score); }
		}
		
		protected virtual void handlePlayerLostEvent(int playerID) 
		{
			Log("handlePlayerLostEvent");
			
			foreach (var handler in parent.playerLostHandlers)
			{ handler(playerID); }
		}
		
		protected virtual void handlePlayerWonEvent(int playerID) 
		{
			Log("handlePlayerWonEvent");
			
			foreach (var handler in parent.playerWonHandlers)
			{ handler(playerID); }
		}
		
		#endregion ABSTRACT_MEMBER_FUNCTIONS


		#region PRIVATE_MEMBER_VARIABLES
		
		public TowerGameManager parent;
		
		protected static void LogMalformedEventContent(string eventName, int senderID)
		{
			Error("Received " + eventName + " with malformed event content from playerID=" + senderID + ".");
		}
		
		#endregion PRIVATE_MEMBER_VARIABLES
		
		private static void Log(object obj)
        {
            Debug.Log("TowerGameManagerImpl: " + obj.ToString());
        }
		
        private static void Error(object obj)
        {
            Debug.LogError("MasterTowerGameManagerImpl: " + obj.ToString());
        }

	}
}