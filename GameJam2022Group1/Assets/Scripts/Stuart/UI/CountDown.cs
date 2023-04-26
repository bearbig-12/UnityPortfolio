using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StuartH
{
    /// <summary>
    ///CountDown - Display countdown timer to start game
    /// </summary>
    public class CountDown : MonoBehaviour
    {
        [SerializeField] private PlayerMovement player;
        [SerializeField] private Image image;
        [SerializeField] private List<Sprite> countDownSprites;
        public static event Action OnGameStart;
        private int index = 0;
        [SerializeField] private Ease easeType;
        private void Awake()
        {
            if (player == null) player = FindObjectOfType<PlayerMovement>();
            player.SetEnabled(false);
            index = countDownSprites.Count;

        }

        private void Start() => InvokeRepeating(nameof(ChangeSprite), 0, 1f);

        private void ChangeSprite()
        {
            if (index == 0)
            {
                OnGameStart?.Invoke();
                //player.SetEnabled(true);
                Destroy(gameObject);
                return;
            }

            index--;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one , 0.3f).SetEase(easeType);
            image.sprite = countDownSprites[index];
        }

        public void Show()
        {
            if (image != null) image.enabled = true;
        }
        
        public void Hide()
        {            if (image != null) image.enabled = false;

        }
        
    }
}
