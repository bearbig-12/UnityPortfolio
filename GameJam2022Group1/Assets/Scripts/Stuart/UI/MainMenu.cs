using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StuartH
{
    /// <summary>
    ///MainMenu - Main menu UI control
    /// </summary>
    public class MainMenu : CanvasGroupBase
    {
        [SerializeField] private ScoreHolder scoreHolder;
         private static ScoreHolder SCOREHOLDER;

        [SerializeField] private HighscoresUI highscoresUI;
        public void NewGame() => SceneManager.LoadScene(2);

        private void Awake()=>SCOREHOLDER = scoreHolder;
        

        public void Highscores()
        {
            Hide();
            highscoresUI.Show(0.2f);
        }
        public void Show(float v=0f) => ShowUI(v);
        public void Hide(float v=0f) => HideUI(v);
        public void QuitUI() => Quit();
        public static void Quit()
        {
            
            SaveFileHandler.Save(SCOREHOLDER);
#if UNITY_EDITOR
            Debug.Log("Quitting application");
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

  

    }
}
