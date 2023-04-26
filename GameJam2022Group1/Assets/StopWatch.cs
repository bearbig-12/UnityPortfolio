using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using StuartH;
using TMPro;

public class StopWatch : MonoBehaviour
{
    // Variables
   static bool timerActive = false;
    static float currentTime;
    public TextMeshProUGUI currentTimeText;
    [SerializeField] private ScoreHolder highscore;
    private PlayerMovement player;
    private void OnDestroy()=>highscore.currentScore.time = currentTime;
    private void OnEnable()
    {CountDown.OnGameStart += StartTimer;
        player.OnDeath += Stop;
    }

    private void Stop() => timerActive = false;

    private void OnDisable()
    {
        player.OnDeath -= Stop;
        CountDown.OnGameStart -= StartTimer;
    } 
    public static float GetCurrentTime() => currentTime;




    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        currentTime = 0;
    } 

    public void StartTimer()
    {
        timerActive = true;
        currentTime = 0;

        StartCoroutine(UpdateTimeCor());
    }

    private IEnumerator UpdateTimeCor()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateTimeDisplay();
        StartCoroutine(UpdateTimeCor());

    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive )
        {
            currentTime +=Time.deltaTime;
        }
    }

    private void UpdateTimeDisplay()
    {
        var time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:ff");
    }
}
