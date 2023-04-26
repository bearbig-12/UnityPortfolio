using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    MainMenu menu;
    public CharacterController characterController;
    public float speed = 50f;


    Vector3 playerVelocity;
    public float Gravity = -50f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;

    public float Jump_Speed = 10f;
    public float Sprint_Speed = 100f;
    bool isGrounded;

    public float Player_HP= 100f;
    // Start is called before the first frame update
    PlayerHPUI Current_playerHP;

    Gun NumberOfEnemy;
    void Start()
    {
        menu = GetComponent<MainMenu>();
        NumberOfEnemy = GetComponent<Gun>();
        Current_playerHP = GetComponent<PlayerHPUI>();
        Player_HP = 100f;

    }

    // Update is called once per frame
    void Update()
    {
        if(Player_HP <= 0)
        {
            menu.GameOver();
        }
        SetHP();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 playerMove = transform.right * x + transform.forward * z;

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerVelocity.y = Jump_Speed;
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = Sprint_Speed;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 12f;
            }
        }
        

        characterController.Move(playerMove * speed * Time.deltaTime);

        playerVelocity.y += Gravity * Time.deltaTime * 5f;

        characterController.Move(playerVelocity * Time.deltaTime);

        Invoke("P_HP", 3f);

    }
    void SetHP()
    {
        Current_playerHP.PlayerHP.text = Player_HP.ToString();
    }
   
 
    public void getDamage(float damage)
    {
        Player_HP -= damage;
    }

}
