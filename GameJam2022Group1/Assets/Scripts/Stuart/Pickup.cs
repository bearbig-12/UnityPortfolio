using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using StuartH;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///Pickup - A pickup item that can be picked up by the player
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private int amount;

        private void OnTriggerEnter(Collider other)
        {
            GoldTracker.Pickup(amount <= 0 ? 1 : amount);
            Destroy(gameObject);
        }
    }
}
