using System;

namespace TowerVR
{
	public class GameState
	{
		public const int AwaitingPlayers 	= 0;
		public const int AllPlayersReady 	= 1;
		public const int Countdown 			= 2;
		public const int Running 			= 3;
		public const int Ended 				= 4;
		public const int Stopped 			= 5;
		
		public static bool IsValid(int potentialGameState)
		{
			return potentialGameState >= AwaitingPlayers && 
				   potentialGameState <= Stopped;
		}
	}

	public class TurnState
	{
		public const int NotStarted 				= 0;
		public const int SelectingTowerPiece 		= 1;
		public const int PlacingTowerPiece			= 2;
		public const int TowerReacting 				= 3;
		
		public static bool IsValid(int potentialTurnState)
		{
			return potentialTurnState >= NotStarted && 
				   potentialTurnState <= TowerReacting;
		}
	}

	public class TowerState
	{
		public const int Stationary 				= 0;
		public const int Moving				 		= 1;
		public const int IncreasingHeight			= 2;
		public const int Falling	 				= 3;

		public static bool IsValid(int potentialTurnState)
		{
			return potentialTurnState >= Stationary && 
				potentialTurnState <= Falling;
		}
	}
}