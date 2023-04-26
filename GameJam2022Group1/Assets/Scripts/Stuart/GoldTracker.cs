using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StuartH
{
    /// <summary>
    ///GoldTracker - DisplaysGold
    /// </summary>
    public class GoldTracker : MonoBehaviour
    {
        private static int totalAmount;
        public static event Action<int> OnPickup;
        [FormerlySerializedAs("score")] [SerializeField] private ScoreHolder highscore;

        private void OnDestroy()=>highscore.currentScore.gold = totalAmount;
        

        public static void Pickup(int amount)
        {
            totalAmount += amount;
            OnPickup?.Invoke(totalAmount);
        }
    }
}