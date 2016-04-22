using UnityEngine;
using System.Collections;

namespace TowerVR
{
	/**
	 * Class representing a playable/placable tower piece.
	 * 
	 * It contains a reference to a mesh as well as a difficulty.
	 * */
	public class TowerPieceInfo : MonoBehaviour
	{
		public Mesh mesh;

		public TowerPieceDifficulty difficulty;
	}
}