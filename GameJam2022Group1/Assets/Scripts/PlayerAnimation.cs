using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StuartH;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] PlayerMovement playerMovement;
    private void OnEnable() => CountDown.OnGameStart += OnGameStart;

    private void OnDisable()=> CountDown.OnGameStart -=OnGameStart;

    private void OnGameStart() => SetEnabled(true);

    private void SetEnabled(bool b)
    {
        anim.SetBool("isWalking", true);
    }

    void Start()
    {
        anim.SetBool("isWalking", false);

        anim.SetBool("isDead", false);
        anim.SetBool("isJumping", false);
    }

    public void PlayDeathAnimation()
    {
        anim.SetBool("isDead", true);
    }

    void PlayJumpingAnimation()
    {
        anim.SetBool("isJumping", true);
    }

    void PlayWalkAnimation()
    {
        anim.SetBool("isJumping", false);
    }

    void Update()
    {
        if (playerMovement.JumpVelocity > 0)
        {
            PlayJumpingAnimation();
        }

        if(playerMovement.JumpVelocity < 0)
        {
            PlayWalkAnimation();
        }
    }
}
