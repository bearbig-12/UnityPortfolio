using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StuartH
{

	/// <summary>
	///HighscoresEntryUI - Used to display a single highscore entry
	/// </summary>

	public class HighscoresEntryUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI rankText;
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private TextMeshProUGUI coinText;
		[SerializeField] private TextMeshProUGUI totalScoreText;

		public void UpdateScore(ScoreHolder.ScoreSaveData highscore, int rank)
		{
			if (highscore == null) return;
			rankText.text = "#" + (rank + 1);
			TimeSpan time = TimeSpan.FromSeconds(highscore.time);
			timeText.text = time.ToString("hh':'mm':'ss");
			coinText.text = highscore.gold.ToString();
			totalScoreText.text = highscore.GetTotalScore().ToString();
		}
	}
}