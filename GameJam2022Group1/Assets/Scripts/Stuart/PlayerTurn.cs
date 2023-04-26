using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///PlayerTurn - Handles player turn
    /// </summary>
    public class PlayerTurn : MonoBehaviour
    {
        [SerializeField] private TurnUI turnUI;
        [SerializeField] private float hideSpeed = 0.3f;
        public event Action<bool> OnTurnEvent; 
        public void TriggerTurnEvent()
        {
            OnTurnEvent?.Invoke(true);

            turnUI.Show(this);
        } 

        private void Awake()=>turnUI.Hide(0);
        

        public void TurnLeft()
        {
            OnTurnEvent?.Invoke(false);
            transform.Rotate(Vector3.up, -90f);
            turnUI.Hide(hideSpeed);
        }

        public void TurnRight()
        {
            OnTurnEvent?.Invoke(false);

            transform.Rotate(Vector3.up, 90f);
            turnUI.Hide(hideSpeed);
        }

    }
}
