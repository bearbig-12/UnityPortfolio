
using System.Collections;
using UnityEngine;

public class BulletHitBox : MonoBehaviour
{
    EnemySpawner NumberOfZombie;
    private void OnTriggerEnter(Collider other)
    {
        NumberOfZombie = GameObject.Find("ZombieSpawner").GetComponent<EnemySpawner>();
        if (other.GetComponentInParent<MyAI>() != null)
        {
            other.GetComponentInParent<MyAI>().ZombieDie();
            Destroy(other.gameObject, 2f);
            NumberOfZombie.Current_Enemy--;
        }





    }




}
