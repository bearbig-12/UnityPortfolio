
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            other.GetComponentInParent<PlayerMovement>().getDamage(10f);
        }
    }
}