using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace TowerVR
{
	public class Scoreboard : TowerGameBehaviour {
		public Text text;
		public string scoreboardTopText = "Scoreboard:";
		
		private IDictionary<PhotonPlayer, Score> scores = new Dictionary<PhotonPlayer, Score>(); 
		
		private void onScoreUpdated(int playerID, Score score)
		{
			var player = PhotonPlayer.Find(playerID);
			if (player == null)
			{
				Debug.LogError("Received ScoreUpdatedEvent with invalid playerID " + playerID);
				return;
			}
			
			scores[player] = score;
			
			
			updateScoreboard();
		}
		
		private void updateScoreboard()
		{
			string scoreboard = "";
			
			// add header text	
			scoreboard += scoreboardTopText + '\n' + '\n';
			
			// copy players and scores to list
			var playerScoreList = new List<KeyValuePair<PhotonPlayer, Score>>();
			foreach (var playerScorePair in scores)
			{
				playerScoreList.Add(playerScorePair);
			}
			
			// sort list by score
			playerScoreList.Sort(delegate (KeyValuePair<PhotonPlayer, Score> x, KeyValuePair<PhotonPlayer, Score> y) {
				var xs = x.Value.score;
				var ys = y.Value.score;
				
				if (xs > ys) return -1;
				if (xs == ys) return 0;
				return 1;
			});
			
			// print each player/score to the scoreboard
			foreach (var playerScorePair in playerScoreList)
			{
				var player = playerScorePair.Key;
				var score = playerScorePair.Value.score;
				
				scoreboard += player.name + ": " + "<b>" + score + "</b>";
				scoreboard += '\n';
			}
			
			text.text = scoreboard;
		}
		
		
		void Start () {
			manager.scoreUpdatedHandlers.Add(onScoreUpdated);
			
			foreach (var player in PhotonNetwork.playerList)
			{
				scores[player] = new Score(0);
			}
			
			updateScoreboard();
		}
		
		void OnDestroy()
		{
			manager.scoreUpdatedHandlers.Remove(onScoreUpdated);
		}
	}
}