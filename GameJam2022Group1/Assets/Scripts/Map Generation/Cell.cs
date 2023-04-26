using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Cell(Vector3 _pos)
    {
        position = _pos;
    }

    public bool isObstacle = false;
    public Vector3 position;
}
