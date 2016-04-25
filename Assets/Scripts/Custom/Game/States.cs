using System;

namespace TowerVR
{
	public class GameState
	{
		public const int AwaitingPlayers 	= 0;
		public const int PreparingStart 	= 1;
		public const int Running 			= 2;
		public const int Ended 				= 3;
		public const int Stopped 			= 4;
		
		public static bool IsValid(int potentialGameState)
		{
			return potentialGameState >= 0 && potentialGameState <= 4;
		}
	}

	public class TurnState
	{
		public const int NotStarted 			= 0;
		public const int ChoosingDifficulty 	= 1;
		public const int ChoosingPiece 			= 2;
		public const int PlacingPiece 			= 3;
		public const int TowerReacting 			= 4;
		
		public static bool IsValid(int potentialTurnState)
		{
			return potentialTurnState >= 0 && potentialTurnState <= 4;
		}
	}
}