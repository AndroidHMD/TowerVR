using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public struct Score
	{
		private int _backingScore;
		/**
		 * The actual score, can only be get not set
		 * */
		public int score { get { return _backingScore; } private set { _backingScore = value; } }

		/**
		 * Constructs a score object
		 * */
		public Score(int score)
		{
			this.score = score;
		}

		/**
		 * Computes the sum of two scores
		 * */
		public static Score Add(Score s1, Score s2)
		{
			return new Score(s1.score + s2.score);
		}

		/**
		 * Private mapping from tower piece difficulty to game score.
		 * */
		private static IDictionary difficultyScores = new Dictionary<TowerPieceDifficulty, Score>()
		{
			{TowerPieceDifficulty.Easy, new Score(2)},
			{TowerPieceDifficulty.Medium, new Score(3)},
			{TowerPieceDifficulty.Hard, new Score(4)}
		};

		/**
		 * Get the score of a provided difficulty.
		 * */
		public static Score GetScore(TowerPieceDifficulty towerPieceDifficulty)
		{
			return (Score) difficultyScores[towerPieceDifficulty];
		}
	}
}