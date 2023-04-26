using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StuartH
{
   /// <summary>
   ///ScoreHolder - holds the scores for the game
   /// </summary>
   [CreateAssetMenu(fileName = "AllScores", menuName = "AllScores")]

   public class ScoreHolder : ScriptableObject
   {
      public ScoreSaveData currentScore;
      public ScoreSaveDataList scores;

      public ScoreSaveData AddScore()
      {
         var ns = new ScoreSaveData(currentScore);
         scores.scores.Add(ns);
         return ns;
      }
      

      public ScoreSaveData GetTopScore()
      {
         OrderScores();
         return scores.scores[0];
      }

      public List<ScoreSaveData> GetTopScores(int amount)
      {
         OrderScores();
         var topScores = new List<ScoreSaveData>();
         for (var i = 0; i < amount && i < scores.scores.Count; i++)
         {
            topScores.Add(scores.scores[i]);
         }

         return topScores;
      }

      public void OrderScores()
      {
         scores.scores.RemoveAll(p => p == null);
         scores.scores = scores.scores.OrderByDescending(o => o.GetTotalScore()).ToList();
      }

      public int GetRank(ScoreSaveData highscore)
      {
         if (highscore == null) return -1;
         OrderScores();
         return scores.scores.FindIndex(x => x ==highscore) + 1;
      }

      public void LoadScores(ScoreSaveDataList savedScores)
      {
         scores.scores.Clear();
         scores.scores = savedScores.scores;
         Debug.Log("Loaded scores");

      }
      
      [Serializable]
      public class ScoreSaveData
      {
         public int gold;
         public float time;

         public ScoreSaveData(int gold, float time)
         {
            this.gold = gold;
            this.time = time;
         }
         public ScoreSaveData(ScoreSaveData s)
         {
            gold = s.gold;
            time = s.time;
         }

         public int GetTotalScore()=>(int)(gold == 0 ? (time *1f)/4f : (time *gold)/4f);
         

         public void Clear()
         {
            gold = 0;
            time = 0f;
         }
      }
	
      [Serializable]
      public class ScoreSaveDataList
      {
         public List<ScoreSaveData> scores = new List<ScoreSaveData>();
      }

      public void ClearCurrent()
      {
         currentScore.Clear();
      }
   }
}
