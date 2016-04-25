using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public abstract class TowerGameManagerImpl : Singleton<TowerGameManagerImpl>, ITowerGameManager
	{
		#region PUBLIC_MEMBER_FUNCTIONS
		
		public void notifyIsReady()
		{
			var ev = new PlayerReadyEvent();
			if (!ev.trySend())
			{
				Debug.LogError(ev.trySendError);
			}
		}
        
        public void tryStartGame()
		{
			var ev = new TryStartGameEvent();
			if (!ev.trySend())
			{
				Debug.LogError(ev.trySendError);
			}
		}
		
		void Awake()
		{
			PhotonNetwork.OnEventCall += _onEventHandler;
		}
		
		void OnDestroy()
		{
			PhotonNetwork.OnEventCall -= _onEventHandler;
			parent = null;
		}
		
		public void _onEventHandler(byte eventCode, object content, int senderID)
		{
			switch (eventCode)
			{
				case NetworkEventCodes.PlayerReady:
				{
					_handlePlayerReadyEvent(senderID); 
					break;	
				}
				case NetworkEventCodes.TryStartGame:
				{
					_handleTryStartGameEvent(senderID); 
					break;	
				}
				case NetworkEventCodes.GameStateChanged:
				{
					int gameState;
					if (GameStateChangedEvent.TryParse(content, out gameState))
					{
						_handleGameStateChangedEvent(gameState);
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
						_handleTurnStateChangedEvent(turnState);
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
						_handleNextPlayerEvent(nextPlayerID);
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
						_handleScoreChangedEvent(playerID, score);
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
						_handlePlayerLostEvent(losingPlayerID);
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
						_handlePlayerWonEvent(winningPlayerID);
					} 
					else
					{
						LogMalformedEventContent("PlayerLostEvent", senderID);
					} 
					break;	
				}
				case NetworkEventCodes.SpawnTowerPiece:
				{
					// todo
					_handleSpawnTowerPieceEvent(); 
					break;	
				}
				default:
					return;
			}
		}
		
		#endregion PUBLIC_MEMBER_FUNCTIONS
		

		#region ABSTRACT_MEMBER_FUNCTIONS
		
		//////////////////////////////////
		/// PhotonNetworkEvent handles ///
		//////////////////////////////////
		
		protected virtual void _handlePlayerReadyEvent(int playerID) {}
		
		protected virtual void _handleTryStartGameEvent(int playerID) {}
		
		protected virtual void _handleGameStateChangedEvent(int gameState) 
		{
			foreach (var handler in parent.gameStateChangedHandlers)
			{ handler(gameState); }
		}
		
		protected virtual void _handleTurnStateChangedEvent(int turnState) 
		{
			foreach (var handler in parent.turnStateChangedHandlers)
			{ handler(turnState); }
		}
		
		protected virtual void _handleNextPlayerEvent(int nextPlayerID) 
		{
			foreach (var handler in parent.nextPlayerTurnHandlers)
			{ handler(nextPlayerID); }
		}
		
		protected virtual void _handleScoreChangedEvent(int playerID, Score score) 
		{
			foreach (var handler in parent.scoreUpdatedHandlers)
			{ handler(playerID, score); }
		}
		
		protected virtual void _handlePlayerLostEvent(int playerID) 
		{
			foreach (var handler in parent.playerLostHandlers)
			{ handler(playerID); }
		}
		
		protected virtual void _handlePlayerWonEvent(int playerID) 
		{
			foreach (var handler in parent.playerWonHandlers)
			{ handler(playerID); }
		}
		
		protected virtual void _handleSpawnTowerPieceEvent() {}
		
		#endregion ABSTRACT_MEMBER_FUNCTIONS


		#region PRIVATE_MEMBER_VARIABLES
		
		public TowerGameManager parent;
		
		private void LogMalformedEventContent(string eventName, int senderID)
		{
			Debug.LogError("Received " + eventName + " with malformed event content from playerID=" + senderID + ".");
		}
		
		#endregion PRIVATE_MEMBER_VARIABLES

	}
}