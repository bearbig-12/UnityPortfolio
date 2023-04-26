using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numberToSpwan;
    public GameObject objectToSpanw;
    public int Current_Enemy;
    public List<GameObject> Number_Of_Enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        numberToSpwan = 14;
        Number_Of_Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        Current_Enemy = Number_Of_Enemies.Count;

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(Current_Enemy);
        
        if (Current_Enemy < 5)
        {
            for (int i = 0; i < numberToSpwan; i++)
            {
                Instantiate(objectToSpanw, transform.position, Quaternion.identity, transform);
                Current_Enemy++;
            }

        }
    }
}
