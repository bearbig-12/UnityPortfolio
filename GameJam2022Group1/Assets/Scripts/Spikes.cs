using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Transform player;
    public bool hadPlayed = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hadPlayed)
        {
            if (Vector3.Distance(gameObject.transform.position, player.position) < 7.0f)
            {
                // trigger spikes animation
                animator.SetBool("isTriggered", true);
                hadPlayed = true;

            }

        }

    }
}
