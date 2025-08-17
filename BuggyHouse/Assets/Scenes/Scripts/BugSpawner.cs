using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugObject;
    public BoxCollider spawnArea;
    public float spawnInterval = 0.5f;

    private float timer;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            SpawnBug();
            timer = 0f;
        }
    }

    void SpawnBug()
    {
        if (GameMode.Instance.isGameOver != true)
        {
            if (spawnArea == null)
            {
                return;
            }

            Vector3 center = spawnArea.bounds.center;
            Vector3 size = spawnArea.bounds.size;

            // 박스 안 랜덤 좌표 뽑기 (2D니까 z는 0으로 고정)
            float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
            float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);

            Vector3 spawnPos = new Vector3(x, y, 0f);

            Quaternion spawnRot = Quaternion.identity;

            string tag = spawnArea.tag;

            if (tag == "Top")
            {
                spawnRot = Quaternion.Euler(0, 0, 180f); // 아래로 이동
            }
            else if (tag == "Bottom")
            {
                spawnRot = Quaternion.Euler(0, 0, 0f);   // 위로 이동
            }
            else if (tag == "Right")
            {
                spawnRot = Quaternion.Euler(0, 0, 90f);  // 왼쪽으로 이동
            }
            else if (tag == "Left")
            {
                spawnRot = Quaternion.Euler(0, 0, -90f); // 오른쪽으로 이동
            }

            Instantiate(bugObject, spawnPos, spawnRot);
        }
    }
      
}
