using System.Collections;

namespace TowerVR
{
	public class NetworkEventCodes
	{
		
		public const byte PlayerReady 		= 1;
		public const byte TryStartGame 		= 2; 
		
		public const byte GameStateChanged 	= 3;
		public const byte TurnStateChanged 	= 4;
		public const byte ScoreChanged 		= 5;
		public const byte PlayerLost 		= 6;
		public const byte PlayerWon 		= 7;
		
		public const byte SpawnTowerPiece 	= 0;
	}
}