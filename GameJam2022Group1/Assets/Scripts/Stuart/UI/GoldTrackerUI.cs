using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///GoldTrackerUI - DisplaysGold
    /// </summary>
    public class GoldTrackerUI : CanvasGroupBase
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string preText = "Gold: ";
        private void OnEnable()
        {
            GoldTracker.OnPickup += PickUp;
            CountDown.OnGameStart += GameStart;
        }
        private void OnDisable()
        {
            GoldTracker.OnPickup -= PickUp;
            CountDown.OnGameStart -= GameStart;

        }
        private void Start() => HideUI();
        private void GameStart()=>ShowUI();
        private void Awake() => PickUp(0);
        private void PickUp(int amount)=>text.text = preText + amount;
        
    }
}