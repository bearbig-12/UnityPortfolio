using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class MyAI : MonoBehaviour
{
    enum ZombieState
    {
        Patrol,
        Chase,
        Attack,
        Die,
        Stop
    }
    NavMeshAgent agent;
    Animator anim;
    public Transform target;
    GameObject player;

    public float view_distance = 5f;
    public float view_Angle = 45f;
    public LayerMask PlayerMask;
    

    // For patrolling
    public Transform[] WayPoints;
    int c_wayPoints;
    Vector3 TargetWaypoint;


    ZombieState Current_State;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        // If the player Tag object is in the scene, the zombie objects move to the player tag object.
        target = GameObject.FindGameObjectWithTag("Player").transform;


        Current_State = ZombieState.Patrol;
   


        List<GameObject> waypointsInLevel = new List<GameObject>();
        waypointsInLevel.AddRange(GameObject.FindGameObjectsWithTag("WayPoints"));
        for (int n = 0; n < waypointsInLevel.Count; n++)
        {
            WayPoints[n] = waypointsInLevel[n].transform;
        }


        UpdateWayPoints();




    }

    // Update is called once per frame
    void Update()
    {
        switch (Current_State)
        {
            case ZombieState.Patrol:
                if (agent.remainingDistance < 0.1f)
                {
                    //Debug.Log("Agent is Stopped");
                    UpdateWayPoints();
                    Patrol();
                }
                Patrol();
                break;
            case ZombieState.Chase:
                anim.SetBool("isChase", true);
                agent.SetDestination(target.position);
                anim.SetBool("isAttacking", false);
                anim.speed = agent.velocity.magnitude / 2f;
                if (Vector3.Distance(player.transform.position, transform.position) > 20f)
                {
                    Current_State = ZombieState.Patrol;
                    UpdateWayPoints();
                }
                if(Vector3.Distance(player.transform.position, transform.position) < 1f)
                {
                    Current_State = ZombieState.Attack;
                }
                break;
            case ZombieState.Attack:
                anim.SetBool("isChase", false);
                anim.SetBool("isAttacking", true);
                anim.speed = 2f;
                if (Vector3.Distance(player.transform.position, transform.position) > 2f)
                {
                    Current_State = ZombieState.Chase;
                }
                break;
            case ZombieState.Die:
                anim.SetBool("isDie", true);
                anim.SetBool("isChase", false);
                anim.SetBool("isAttacking", false);
                Invoke("AgentStop", 1f);
                gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
            case ZombieState.Stop:
                agent.isStopped = true;
                anim.enabled = false;
                break;

        }

        //if (GameObject.Find("Player").GetComponent<PlayerAI>().isPlayerDie == true)
        //{
        //    Current_State = ZombieState.Stop;

        //}
        if (Input.GetKeyDown(KeyCode.J))
        {
            Current_State = ZombieState.Die;

        }

        EnenmyViewAndChase();



    }

    void UpdateWayPoints()
    {
        int index = Random.Range(0, WayPoints.Length);
        //Debug.Log(index);
        c_wayPoints = index;
        TargetWaypoint = WayPoints[c_wayPoints].position;
    }

    void Patrol()
    {
        anim.SetBool("isChase", false);
        anim.SetBool("isAttacking", false);
        agent.SetDestination(WayPoints[c_wayPoints].position);
    }

    public void ZombieDie()
    {
        Current_State = ZombieState.Die;


    }
    void AgentStop()
    {
        agent.isStopped = true;
        agent.enabled = true;

    }
   

    void EnenmyViewAndChase()
    {
        // Check the player's collider
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, view_distance, PlayerMask);

        for(int i = 0; i < playerCollider.Length; i++)
        {
            Transform PlayerTransform = playerCollider[i].transform;

            Vector3 DirToPlayer = (PlayerTransform.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, DirToPlayer) < view_Angle * 0.5)
            {
                
                if (Physics.Raycast(transform.position, DirToPlayer, out RaycastHit hit, view_distance))
                {
                    //        //Debug.Log(hit.point + hit.collider.gameObject.name);

                    Debug.Log(hit.transform.tag);
                    if (hit.transform.name == "Player")
                    {
                        //isOnSight = true;
                        //isPatrol = false;
                        Current_State = ZombieState.Chase;

                    }

                }
            }


        }
    }
}
