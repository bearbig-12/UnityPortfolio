using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///TurnTrigger - Handles triggering of events for turning
    /// </summary>
    /// 
    public class TurnTrigger : MonoBehaviour
    {
        private bool triggered = false;
        private void Awake() => triggered = false;
        private void OnEnable() => triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerTurn player)) return;
            player.TriggerTurnEvent();
        }
    }
}
