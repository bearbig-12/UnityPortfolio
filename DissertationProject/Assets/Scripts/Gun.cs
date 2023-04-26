
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 5f;
    public float gun_range = 100f;
    bool AttackTerm;
    /// <summary>
    /// public float fire_Rate = 15f;
    /// </summary>
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject GunImpactEffect;

    public int Current_Eenemy;
    PlayerHPUI Current_Num_Enemy;
    MainMenu menu;

    void Start()
    {
        menu = GetComponent<MainMenu>();
        Current_Num_Enemy = GetComponent<PlayerHPUI>();
    }
    void Update()
    {
        if (Current_Eenemy == 0)
        {
            menu.NextLevel();
        }
        EnemyCount();
        //if(Input.GetButton("Fire1") && (Time.time >= nextTimeToFire))
        //{
        //    nextTimeToFire = Time.time + 1f / fire_Rate;
        //    Shoot();
        //}

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (AttackTerm == false)
        {
            muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, gun_range))
            {
                //Debug.Log(hit.transform.name);
                EnemyAI target = hit.transform.GetComponent<EnemyAI>();
                //FuzzyEnemyAI target = hit.transform.GetComponent<FuzzyEnemyAI>();

                if (target != null)
                {
                    target.DamageTake(damage);
                    if (target.HP <= 0)
                    {
                        target.Die();
                        Current_Eenemy--;
                        Destroy(target.gameObject);
                    }
                }
            }
            GameObject GunImpact = Instantiate(GunImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(GunImpact, 1f);
            AttackTerm = true;
            Invoke("NextAttackTerm", 0.2f);

        }

    }

    void NextAttackTerm()
    {
        AttackTerm = false;
    }
    void EnemyCount()
    {
        Current_Num_Enemy.Number_Of_Enemy.text = Current_Eenemy.ToString();
    }
}
