using System.Collections;

namespace TowerVR
{
	/**
	 * Constant class containing event codes. The Photon API specifies the
	 * event codes as bytes within the [0, 199] range.
	 **/
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
		
		public static bool IsValid(byte potentialEventCode)
		{
			return potentialEventCode >= 0 && potentialEventCode <= 7;
		}
	}
}