using System;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///Tile - Handles triggering of tile deletion
    /// </summary>
    public class Tile : MonoBehaviour
    {
        private Action<GameObject> deleteCallback;
        public void Init(Action<GameObject> deleteCallback) => this.deleteCallback = deleteCallback;

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerMovement player)) return;
            if (deleteCallback == null)
            {
                Debug.Log("missing callback");
                return;
            }

            deleteCallback?.Invoke(gameObject);
        }
    }
}