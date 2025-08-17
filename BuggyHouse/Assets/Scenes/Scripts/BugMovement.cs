using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public float moveSpeed = 1f;       // 이동 속도
    public bool IsDead = false;    
    public float rotationAngle = 45f;  // 회전 각도
    public float rotationInterval = 0.5f; // 몇 초마다 방향 바꿀지
    private float timer = 0f;
    private Animator animator;

    public GameObject splashPrefab; // 스플래쉬 에니메이션 프리팹


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsDead == false && GameMode.Instance.isGameOver == false)
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

            // 화면 밖으로 나가면 오브젝트 삭제
            if (transform.position.x < -8f || transform.position.x > 8f ||
                transform.position.y < -6f || transform.position.y > 6f)
            {
                Destroy(gameObject);
            }
        }
        if(GameMode.Instance.isGameOver == true)
        {
            // 게임 오버 되면 에니메이터 멈추기
            animator.enabled = false;
        }
     

    }

    void OnMouseDown() // 클릭했을 때
    {
        if (GameMode.Instance.isGameOver != true)
        {
            if (!IsDead)
            {
                IsDead = true;
                moveSpeed = 0f;

                // 애니메이터 전환
                animator.SetBool("IsClicked", true);
                // 킬카운트 추가
                GameMode.Instance.AddKill();

                // 스플래쉬 이펙트 생성
                GameObject splash = Instantiate(splashPrefab, transform.position, Quaternion.identity);
                Destroy(splash, 1.0f); // 애니메이션 길이에 맞게 조절

                // 벌레 시체도 일정 시간 뒤 삭제
                Destroy(gameObject, 1.0f);
            }
        }
    }
       
}
