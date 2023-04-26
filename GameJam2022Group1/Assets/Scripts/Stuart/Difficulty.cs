using System;
using System.Collections;
using System.Collections.Generic;
using StuartH;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///Difficulty - Updates difficulty based on time
    /// </summary>
    public class Difficulty : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        [SerializeField] private float targetTimeMins;
        [SerializeField] private InfiniteWorld infiniteWorld;
        private void Awake() => playerMovement = FindObjectOfType<PlayerMovement>();
        private void Start() => targetTimeMins *= 60;

        private void Update()
        {
            playerMovement.SetSpeed(Mathf.Lerp(playerMovement.GetPlayerMinSpeed(), playerMovement.GetPlayerMaxSpeed(),
                StopWatch.GetCurrentTime()/targetTimeMins));
            playerMovement.SetPanSpeed(Mathf.Lerp(playerMovement.GetPlayerMinPanSpeed(),
                playerMovement.GetPlayerMaxPanSpeed(), StopWatch.GetCurrentTime()/targetTimeMins));
            infiniteWorld.SetCornerRate(Mathf.Lerp(infiniteWorld.GetMinCornerAmount(), infiniteWorld.GetMaxCornerAmount(),
                StopWatch.GetCurrentTime()/targetTimeMins));
        }
    }
}