using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private HeathBar HP_Bar;
    float Max_HP;
    enum State
    {
        Patrol,
        Chase,
        Attack,
        RunAway,
        Die

    }

    State Current_State;

    enum WeaponType
    {
        Melee,
        Rifle,
        Sniper
    }
    WeaponType Current_Weapon;
    public int Weapon_Type; 

    NavMeshAgent agent;
    public Transform target;
    GameObject player;

    Animator anim;


    bool isOnSight;

    public float HP;

    //Patrol
    public Transform[] WayPoints;
    int c_wayPoints;
    Vector3 TargetWaypoint;

    // Attack
    public float AttackTerm;
    bool Attacked;

    public float SightRange, AttackRange;

    public LayerMask PlayerMask;


    public GameObject Bullet;
    public Transform Bullet_Point;
    public float Bullet_Speed = 20000f;


    // Start is called before the first frame update
    void Start()
    {

        Max_HP = 100;
        HP = 100;

        HP_Bar.UpdateHPBar(Max_HP, HP);

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        HP = 100f;
        anim = GetComponent<Animator>();



        if (Weapon_Type == 0)
        {
            Current_Weapon = WeaponType.Melee;
            SightRange = 40;
            AttackRange = 10;
        }
        else if(Weapon_Type == 1) 
        {
            Current_Weapon = WeaponType.Rifle;
            SightRange = 60;
            AttackRange = 40;
        }
        else
        {
            Current_Weapon = WeaponType.Sniper;
            SightRange = 100;
            AttackRange = 70;
        }
      


        

        //List<GameObject> waypointsInLevel = new List<GameObject>();
        //waypointsInLevel.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
        //for (int n = 0; n < waypointsInLevel.Count; n++)
        //{
        //    WayPoints[n] = waypointsInLevel[n].transform;
        //}

        Current_State = State.Patrol;
        UpdateWayPoints();

        isOnSight = false;
        Attacked = false;
    }

    void Update()
    {
        HP_Bar.UpdateHPBar(Max_HP, HP);

        //agent.SetDestination(target.position);

        EnenmyView();

        if (Vector3.Distance(transform.position, player.transform.position) < AttackRange && HP > 20)
        {
            Current_State = State.Attack;
        }

        switch (Current_State)
        {
            case State.Patrol:
                anim.SetBool("isPatroling", true);
                anim.SetBool("isChasing", false);
                anim.SetBool("isShooting", false);
                agent.speed = 10f;
                if (agent.remainingDistance < 7f)
                {
                    //Debug.Log("Agent is Stopped");
                    UpdateWayPoints();
                    Patrol();
                }
                Patrol();
                break;
            case State.Chase:
                if(Current_Weapon == WeaponType.Melee) 
                {
                    agent.speed = 40f;
                }
                else
                {
                    agent.speed = 25f;
                }
                anim.SetBool("isChasing", true);
                anim.SetBool("isPatroling", false);
                anim.SetBool("isShooting", false);
                agent.SetDestination(target.position);
                if (Vector3.Distance(player.transform.position, transform.position) > SightRange)
                {
                    UpdateWayPoints();
                    Current_State = State.Patrol;
                    isOnSight = false;
                }
                break;
            case State.Attack:
                if (Current_Weapon == WeaponType.Melee)
                {
                    agent.speed = 40f;
                }
                else
                {
                    agent.speed = 25f;
                }
                Attack();
                if (Vector3.Distance(transform.position, player.transform.position) > AttackRange)
                {
                    Current_State = State.Chase;
                }
                break;
          
            case State.RunAway:
                agent.speed = 50f;
                RunAway();
                if (Vector3.Distance(transform.position, player.transform.position) > SightRange)
                {
                    Current_State = State.Patrol;
                }
                break;
            case State.Die:
                agent.isStopped = true;
                break;
        }



        //Invoke("C_HP", 2f);
    }

   
 
    void Patrol()
    {
        agent.SetDestination(WayPoints[c_wayPoints].position);

    }

    void UpdateWayPoints()
    {
        int index = Random.Range(0, WayPoints.Length);
        //Debug.Log(index);
        c_wayPoints = index;
        TargetWaypoint = WayPoints[c_wayPoints].position;
    }
    void Chase()
    {
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        Debug.Log("Enemy state = Attack");
        agent.SetDestination(target.position);
        if (Attacked == false)
        {
           if(Current_Weapon == 0)
            {
                anim.SetBool("isChasing", false);
                anim.SetBool("isPatroling", false);
                anim.SetBool("isShooting", true);
            }
            else
            {
                anim.SetBool("isChasing", false);
                anim.SetBool("isPatroling", false);
                anim.SetBool("isShooting", true);
                GameObject bullet = Instantiate(Bullet, Bullet_Point.transform.position, Bullet_Point.transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * Bullet_Speed);
                Destroy(bullet, 1);
                Attacked = true;
                Invoke("NextAttackTerm", 1.5f);
            }
         
        }
        //if (Current_Weapon == WeaponType.Melee)
        //{
        //    anim.SetBool("isChasing", false);
        //    anim.SetBool("isPatroling", false);
        //    anim.SetBool("isShooting", true);
        //}
        //if (Current_Weapon == WeaponType.Rifle || Current_Weapon == WeaponType.Sniper)
        //{
           
     
        //}
       
        
       
  
   

        //agent.isStopped = true;
        //agent.SetDestination(target.position);
       
    }
  

    public void DamageTake(float damage)
    {
        HP -= damage;

    }
    public void Die()
    {
        Current_State = State.Die;
    }

    void RunAway()
    {
        anim.SetBool("isChasing", true);
        anim.SetBool("isPatroling", false);
        anim.SetBool("isShooting", false);
        Vector3 dirToPlayer = transform.position - player.transform.position;

        Vector3 RunAwayPos = transform.position + dirToPlayer;

        agent.SetDestination(RunAwayPos);
    }

    void EnenmyView()
    {
        isOnSight = Physics.CheckSphere(transform.position, SightRange, PlayerMask);
        if (isOnSight == true)
        {
            Current_State = State.Chase;
            if(HP <= 20)
            {
                Current_State = State.RunAway;
            }
        }
    }
    void C_HP()
    {
        Debug.Log(HP);
    }
    void NextAttackTerm()
    {
        Attacked = false;
    }
}
