using System;
using System.Collections;
using System.Collections.Generic;
using StuartH;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private int mainMenuIndex = 1;
    [SerializeField] private int gameIndex = 2;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private ScoreHolder scores;

    private void Awake()
    {
      var scoreData= scores.AddScore();
        var rank = scores.GetRank(scoreData);
        TimeSpan time = TimeSpan.FromSeconds(scoreData.time);
        scoreText.text = "You have ranked #" + rank + "\n";
        scoreText.text += "Time: " + time.ToString("hh':'mm':'ss");
        scoreText.text += "\n"+"Gold: " + scoreData.gold + "\n";
        scoreText.text += "Final score: " + scoreData.GetTotalScore();
        SaveFileHandler.Save(scores);
        scores.ClearCurrent();
    }

    public void TryAgain() => SceneManager.LoadScene(gameIndex);
    public void Menu() => SceneManager.LoadScene(mainMenuIndex);
}