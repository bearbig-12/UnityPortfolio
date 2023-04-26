using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///UFOMove - Moves the UFO
    /// </summary>
    public class UFOMove : MonoBehaviour
    {
        [SerializeField] private float y;
        [SerializeField] private float rotSpeed = 30f;

        private void Update()
        {
            transform.Rotate(transform.up, rotSpeed * Time.deltaTime);
            y = Mathf.PingPong(Time.time, 0.5f);
            transform.position = new Vector3(transform.position.x, y + 1.5f, transform.position.z);
        }
    }
}
