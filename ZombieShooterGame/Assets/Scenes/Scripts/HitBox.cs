
using UnityEngine;


public class HitBox : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerAI>() != null)
        {
            other.GetComponentInParent<PlayerAI>().Die();

        }
      
      
    }
 
}
