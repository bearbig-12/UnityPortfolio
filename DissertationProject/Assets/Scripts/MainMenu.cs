using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
   

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Replay()
    {
      
         SceneManager.LoadScene(1);
       
    }
    public void ReStart()
    {

        SceneManager.LoadScene(1);

    }

    public void GameOver()
    {
        SceneManager.LoadScene(4);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
