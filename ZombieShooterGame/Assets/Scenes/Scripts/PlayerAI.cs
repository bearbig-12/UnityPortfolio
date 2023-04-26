using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    Camera cam;

    List<Rigidbody> AllRbs;
    public bool isPlayerDie;

    bool isShooting;
    bool isRunnung;
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private GameObject Bullet_Point;
    public float Bullet_Speed = 20000f;


    //// Start is called before the first frame update
    void Start()
    {
        isPlayerDie = false;
        isShooting = false;
        isRunnung = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;

        AllRbs = new List<Rigidbody>();
        AllRbs.AddRange(GetComponentsInChildren<Rigidbody>());
        foreach (Rigidbody rb in AllRbs) 
        {
            rb.isKinematic = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        if (Input.GetKeyDown(KeyCode.F))
        {
            isShooting = true;
            if (isRunnung == false)
            {
                anim.SetFloat("IsShooting", 1);
            }
            Shooting();
        }
        if (!Input.GetKeyDown(KeyCode.F))
        {
            isShooting = false;
            anim.SetFloat("IsShooting", 0);

        }
    }
    // Based on camera the player object changes their forward dir.
    void LateUpdate()
    {
        if (isPlayerDie == false)
        {
            Vector3 PlayerRot = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerRot), Time.deltaTime * 10f);
        }
       
    }

    void Shooting()
    {
  
        GameObject bullet = Instantiate(Bullet, Bullet_Point.transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * Bullet_Speed);
        Destroy(bullet, 1);
    }
    public void Die()
    {
        foreach(Rigidbody rb in AllRbs)
        {
            rb.isKinematic = false;
        }
        anim.enabled = false;
        agent.isStopped = true;
        isPlayerDie = true;
    }
    void MovementInput()
    {
        if (isPlayerDie == false)
        {
            Vector3 Forward = transform.TransformDirection(Vector3.forward);
            Vector3 Right = transform.TransformDirection(Vector3.right);

            Vector3 moveDir = Forward * Input.GetAxis("Vertical") + Right * Input.GetAxis("Horizontal");

            if ((Input.GetAxis("Vertical") != 0) || Input.GetAxis("Horizontal") != 0)
            {
                anim.SetFloat("forward", 1f);
                isRunnung = true;


            }
            if ((Input.GetAxis("Vertical") == 0) && Input.GetAxis("Horizontal") == 0)
            {
                anim.SetFloat("forward", 0f);
                isRunnung = false;
              

            }

            agent.Move(moveDir.normalized * 10f * Time.deltaTime);
        }
        

    }
}
