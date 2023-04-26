using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///WorldOrigin - Handles world resetting world origin
    /// </summary>
    public class WorldOrigin : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private InfiniteWorld infiniteWorld;
        [SerializeField] private CinemachineVirtualCamera camera;
        [SerializeField] private float maxAxis = 100f;
        [SerializeField] private Vector3 resetPoint;

        private void Awake()
        {
            resetPoint = playerMovement.transform.position;
        }

        private void Update()
        {
            var pos = playerMovement.transform.position;
            if (Mathf.Abs( pos.x) > maxAxis || Mathf.Abs( pos.y) > maxAxis || Mathf.Abs( pos.z) > maxAxis)
            {
                Debug.LogWarning("Resetting origin");
                ResetPlayerPosition();
                ResetCameraPosition(pos);
                ResetWorldPosition(pos);
            }
        }

        private void ResetWorldPosition(Vector3 pos)
        {
            foreach (var t in infiniteWorld.GetAllTiles())
            {
                t.position -= pos;
            }
        }

        private void ResetCameraPosition(Vector3 pos)
        {
            
            camera.OnTargetObjectWarped(playerMovement.transform,camera.transform.position - pos);
            camera.transform.position -= pos;
        }

        private void ResetPlayerPosition()
        {
            playerMovement.enabled = false;
            playerMovement.transform.position = Vector3.zero;
            playerMovement.enabled = true;
        }
    }
}
