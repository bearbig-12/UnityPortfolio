using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletingOldGrid : MonoBehaviour
{

    public Transform player;
    public GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Destroy grid");
            gridManager.DeleteOldGrid();
        }
    }
}
