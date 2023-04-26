using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCheck : MonoBehaviour
{
    public static bool isDead;
    [SerializeField] PlayerAnimation playerAnimation;

    void Start()
    {
        isDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            isDead = true;
            playerAnimation.PlayDeathAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            isDead = true;
            playerAnimation.PlayDeathAnimation();
        }
    }
}