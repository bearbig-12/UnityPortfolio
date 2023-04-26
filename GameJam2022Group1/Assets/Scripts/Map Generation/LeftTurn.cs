using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTurn : MonoBehaviour
{
    public Transform player;
    public GridManager gridManager;
    public Transform nextGridStart;
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
            Debug.Log("Create new grid");
            gridManager.CreateNewGrid(nextGridStart.position, new Vector3(0, gameObject.transform.rotation.eulerAngles.y - 90, 0));
        }
    }
}
