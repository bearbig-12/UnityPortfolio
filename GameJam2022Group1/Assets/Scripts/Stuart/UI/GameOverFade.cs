using System;
using System.Collections;
using System.Collections.Generic;
using StuartH;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StuartH
{
   /// <summary>
   ///GameOverFade - handles the fade to black when the player dies
   /// </summary>
   public class GameOverFade : CanvasGroupBase
   {
      [SerializeField] private PlayerMovement playerMovement;
      [SerializeField] private float fadeTime = 1.5f;
      [SerializeField] private float sceneTransTime = 0.4f;
      private bool isDead = false;
      private void Awake() => HideUI(0);
      private void OnEnable() => playerMovement.OnDeath += GameOver;
      private void OnDisable() => playerMovement.OnDeath -= GameOver;

      private void GameOver()
      {
         if (isDead) return;
         isDead = true;
         ShowUI(fadeTime);
         StartCoroutine(TransCor());
      }

      private IEnumerator TransCor()
      {
         yield return new WaitForSeconds(fadeTime + sceneTransTime);
         SceneManager.LoadScene(3);
      }
   }
}
