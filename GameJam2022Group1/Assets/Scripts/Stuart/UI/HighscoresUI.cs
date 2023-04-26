using System;
using System.Collections;
using System.Collections.Generic;
using StuartH;
using UnityEngine;
using UnityEngine.Serialization;

namespace StuartH
{

    /// <summary>
    ///HighscoresUI - This class is used to display the highscores in the UI
    /// </summary>

    public class HighscoresUI : CanvasGroupBase
    {
        [SerializeField] private GameObject scoreTitlePrefab;

        [SerializeField] private GameObject scoreEntryPrefab;
        [SerializeField] private List<HighscoresEntryUI> highscoresEntryUI;
        [SerializeField] private ScoreHolder scoreHolder;
        [SerializeField] private Transform container;
        [SerializeField] private int scoresToShow = 5;
        [SerializeField] private MainMenu mainMenu;

        private void Awake()
        {
            highscoresEntryUI = new List<HighscoresEntryUI>(scoresToShow);
            Instantiate(scoreTitlePrefab, container);
            Hide();
        }

        private void ClearExistingScores()
        {
            foreach (var hs in highscoresEntryUI)
            {
                if (hs == null) continue;
                Destroy(hs.gameObject);
            }

            highscoresEntryUI.Clear();
        }

        private void UpdateScoreEntries()
        {
            var scores = scoreHolder.GetTopScores(scoresToShow);

            for (var i = 0; i < scores.Count; i++)
            {
                if (scores[i] == null) continue;
                var hse = Instantiate(scoreEntryPrefab, container).GetComponent<HighscoresEntryUI>();
                hse.UpdateScore(scores[i], i);
            }
        }

        public void Return()
        {
            mainMenu.Show(.1f);
            Hide(.1f);
        }

        public void Show(float v = 0f)
        {
            SaveFileHandler.Load(scoreHolder);

            ShowUI(v);
            if (scoreHolder == null) return;
            if (scoreEntryPrefab == null) return;
            ClearExistingScores();
            UpdateScoreEntries();
        }

        public void Hide(float v = 0f) => HideUI(v);
    }
}