using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///TurnUI - Control of in game turn UI
    /// </summary>
    public class TurnUI : CanvasGroupBase
    {
        private PlayerTurn playerTurn;
      [SerializeField]   private PlayerMovement player;

       

        private void OnEnable()=>player.OnDeath += HandlePlayerDeath;

        private void OnDisable()=>player.OnDeath += HandlePlayerDeath;

        private void HandlePlayerDeath()=>HideUI();
        

        public void Show(PlayerTurn playerTurn)
        {
            ShowUI();
            this.playerTurn = playerTurn;
        }

        public void Hide(float hideSpeed) => HideUI(hideSpeed);
        public void Left() => playerTurn.TurnLeft();
        public void Right() => playerTurn.TurnRight();
    }
}
