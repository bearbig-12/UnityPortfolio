using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;     // UI에 TextMeshpro를 사용할 때 


public class GameMode : MonoBehaviour
{
    public static GameMode Instance; //  싱글톤으로 다른 스크립트에서 접근 가능


    [Header("게임 설정")]
    public float gameTime = 10f; 
    private float currentTime;
    public int killCount = 0;
    public bool isGameOver = false;

    [Header("UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killText;

    [Header("게임오버 UI")]
    public GameObject gameOverUI;
    public TextMeshProUGUI finalKillText;


    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTime = gameTime;
        gameOverUI.SetActive(false); 

        UpdateUI();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        currentTime -= Time.deltaTime;

        // 시간 감소
        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timeText != null)
            timeText.text = Mathf.CeilToInt(currentTime).ToString(); 

        if (killText != null)
            killText.text = killCount.ToString();
    }

    public void AddKill()
    {
        killCount++;
        UpdateUI();
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! 총 처치 수 : " + killCount);
        //게임오버 UI 띄우기
        if(gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        if(finalKillText != null)
        {
            finalKillText.text = killCount.ToString();
        }

    }

    public void RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
