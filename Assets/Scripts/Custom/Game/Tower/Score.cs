using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * C# struct variable hold the value, while class variables are references. A score should be a value => struct.
 * */
public struct Score
{
	/**
	 * The actual score, can only be get not set
	 * */
	public uint score { get; }

	/**
	 * Constructs a score object
	 * */
	public Score(uint score)
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
		return difficultyScores[towerPieceDifficulty];
	}
}