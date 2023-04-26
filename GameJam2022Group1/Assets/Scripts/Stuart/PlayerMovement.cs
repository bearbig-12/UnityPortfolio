using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///PlayerMovement - simple player movement controller
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float panSpeed = 1.5f;
        [SerializeField] private bool isActive = true;
        [SerializeField] private float maxMoveAmount = 100f;
        [Range(1f,100f)] [SerializeField] private float minSpeed = 5f;
        [Range(1f,100f)][SerializeField] private float maxSpeed = 40f;
        [Range(1f,10f)] [SerializeField] private float minPanSpeed = 1f;
        [Range(1f,10f)][SerializeField] private float maxPanSpeed = 15;
        public void SetEnabled(bool b) => isActive = b;

        public float GetPlayerMinSpeed() => minSpeed;
        public float GetPlayerMaxSpeed() => maxSpeed;
        public float GetPlayerMinPanSpeed() => minPanSpeed;
        public float GetPlayerMaxPanSpeed() => maxPanSpeed;

        public float GetPanSpeed() => panSpeed;
        public void SetPanSpeed(float f) => panSpeed = f;
        private CharacterController characterController;
        public event Action<float> SpeedChange;
        public event Action OnDeath;
        private PlayerTurn playerTurn;
        private void Awake()
        {
            playerTurn = GetComponent<PlayerTurn>();
            characterController = GetComponent<CharacterController>();
        } 
        public float GetSpeed() => moveSpeed;


        public float JumpVelocity = 0.0f; //made it public to access for animation
        [SerializeField] private float jumpForce;
        [SerializeField] private float mGravity = 19.2f;
        
        [SerializeField] private float mCrouchedHeight = 1.0f;
        [SerializeField] private float mStandingHeight = 2.0f;
        private bool canPan = true;
        private void OnEnable()
        {
            playerTurn.OnTurnEvent += OnTurn;
            CountDown.OnGameStart += OnGameStart;
        }
        private void OnDisable()
        {
            playerTurn.OnTurnEvent -= OnTurn;
            CountDown.OnGameStart -=OnGameStart;
        } 
        private void OnTurn(bool _canPan)=>canPan = !_canPan;
        private void OnGameStart() => SetEnabled(true);

        public void SetSpeed(float s)
        {
            moveSpeed = s;
            SpeedChange?.Invoke(moveSpeed);
        }
        
        private void Update()
        {
            if (!isActive) return;
            var move = transform.forward * moveSpeed;
            if (Input.GetKey(KeyCode.A) && canPan) move -= transform.right * panSpeed;
            if (Input.GetKey(KeyCode.D)&& canPan) move += transform.right * panSpeed;
            characterController.height = Input.GetKey(KeyCode.C) ? mCrouchedHeight : mStandingHeight;
            if (characterController.isGrounded)
            {
                JumpVelocity = -0.5f;
                if (Input.GetKey(KeyCode.Space)) JumpVelocity = 5 * jumpForce;
            }
            else JumpVelocity -= mGravity * Time.deltaTime;
            move += transform.up * JumpVelocity;
            characterController.Move(Time.deltaTime * move);
            if(DeathCheck.isDead) HandleGameOver();
        }

        private void HandleGameOver()
        {
            isActive = false;
            OnDeath?.Invoke();
        }
    }
}