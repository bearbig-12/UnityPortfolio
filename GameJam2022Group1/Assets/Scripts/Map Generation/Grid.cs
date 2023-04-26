using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool DrawGizmos = false;
    [SerializeField] private int Xsize;  // length
    [SerializeField] int Zsize = 3;  // width
    Cell[,] grid;
    [SerializeField] public List<GameObject> parts;
    [SerializeField] public List<GameObject> obstacleParts;
    [SerializeField] public List<GameObject> turnParts; // element 0 = turnR

    // Start is called before the first frame update
    void Start()
    {
        Xsize = Random.Range(25, 35);
        createGrid(Xsize, Zsize);
    }

    public void createGrid(int xSize, int zSize)
    {
        grid = new Cell[xSize, zSize];

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    Cell cell = new Cell(new Vector3(x, 0, z));
                    grid[x, z] = cell;
                }
            }
        PlaceObstacles();
    }

    void PlaceObstacles()
    {
        int randomNum;
        for (int x = 1; x < Xsize-3; x++)
        {
            if (x == 0)
            {
                CreateNormalCell(x, 0);
                CreateNormalCell(x, 1);
                CreateNormalCell(x, 2);
            }
            else if(grid[x-1, 0].isObstacle || grid[x - 1, 1].isObstacle || grid[x - 1, 2].isObstacle)
            {
                CreateNormalCell(x, 0);
                CreateNormalCell(x, 1);
                CreateNormalCell(x, 2);
            }
            else
            {
                randomNum = Random.Range(0, 5);
                //Debug.Log(randomNum);
                // place obstacles in different configurations depending on the randomNum value 
                switch (randomNum)
                {
                    case 0:
                        grid[x, 0].isObstacle = true;
                        CreateObstacle(x, 0);
                        grid[x, 1].isObstacle = false;
                        CreateNormalCell(x, 1);
                        grid[x, 2].isObstacle = false;
                        CreateNormalCell(x, 2);
                        break;
                    case 1:
                        grid[x, 0].isObstacle = false;
                        CreateNormalCell(x, 0);
                        grid[x, 1].isObstacle = true;
                        CreateObstacle(x, 1);
                        grid[x, 2].isObstacle = false;
                        CreateNormalCell(x, 2);
                        break;
                    case 2:
                        grid[x, 0].isObstacle = false;
                        CreateNormalCell(x, 0);
                        grid[x, 1].isObstacle = false;
                        CreateNormalCell(x, 1);
                        grid[x, 2].isObstacle = true;
                        CreateObstacle(x, 2);
                        break;
                    case 3:
                        grid[x, 0].isObstacle = true;
                        CreateObstacle(x, 0);
                        grid[x, 1].isObstacle = true;
                        CreateObstacle(x, 1);
                        grid[x, 2].isObstacle = false;
                        CreateNormalCell(x, 2);
                        break;
                    case 4:
                        grid[x, 0].isObstacle = false;
                        CreateNormalCell(x, 0);
                        grid[x, 1].isObstacle = true;
                        CreateObstacle(x, 1);
                        grid[x, 2].isObstacle = true;
                        CreateObstacle(x, 2);
                        break;
                    case 5:
                        grid[x, 0].isObstacle = true;
                        CreateObstacle(x, 0);
                        grid[x, 1].isObstacle = false;
                        CreateNormalCell(x, 1);
                        grid[x, 2].isObstacle = true;
                        CreateObstacle(x, 2);
                        break;
                }
            }
        }
        // turn -> need walls to indicate new direction
        CreateTurnCell(Xsize - 2, 1);
    }

    void CreateObstacle(int _x, int _z)
    {
        int index = 0;
        //Random.Range(0, length of list);
        GameObject x = Instantiate(obstacleParts[index], grid[_x, _z].position, Quaternion.identity);
        x.transform.SetParent(gameObject.transform, false);
    }

    void CreateNormalCell(int _x, int _z)
    {
        int index = Random.Range(0, parts.Capacity);
        GameObject x = Instantiate(parts[index], grid[_x, _z].position, Quaternion.identity);
        x.transform.SetParent(gameObject.transform, false);
    }

    void CreateTurnCell(int _x, int _z)
    {
        int index = Random.Range(0, 2);
        GameObject x = Instantiate(turnParts[index], grid[_x, _z].position, Quaternion.identity);
        x.transform.SetParent(gameObject.transform, false);
    }
    void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            if (!Application.isPlaying) return;
            for (int x = 0; x < Xsize; x++)
            {
                for (int z = 0; z < Zsize; z++)
                {
                    Cell cell = grid[x, z];
                    if (cell.isObstacle)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }
                    Vector3 pos = new Vector3(x, 0, z);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
        
    }
}
