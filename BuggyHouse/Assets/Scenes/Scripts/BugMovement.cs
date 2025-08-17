using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public float moveSpeed = 1f;       // 이동 속도
    public bool IsDead;    
    public float rotationAngle = 45f;  // 회전 각도
    public float rotationInterval = 0.5f; // 몇 초마다 방향 바꿀지
    private float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if(IsDead == false)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            if(timer >= rotationInterval)
            {
                timer = 0f;
                int dir;
                int random = Random.Range(0, 2);
                if(random == 0 )
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }
                transform.Rotate(0f, 0f, dir * rotationAngle);
            }
        }
    }
}
