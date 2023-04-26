using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    GameObject grid1;
    [SerializeField] GameObject GridPrefab;
    // need last piece position to create new grid there
    // direction???

    List<GameObject> gridsList = new List<GameObject>();

    private void Awake()
    {
        grid1 = Instantiate(GridPrefab, transform.position, Quaternion.identity);
        gridsList.Add(grid1);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gridsList.Count);
        // grid1.GetComponent<Grid>(). get last position to use in the next grid piece
        // Instantiate(GridPrefab, transform.TransformPoint(new Vector3 (0, 1, 1)), transform.rotation) ;

        //grid2 = Instantiate(GridPrefab, transform.position + new Vector3(20,0,0), Quaternion.Euler(0, 90, 0));
    }

    public void CreateNewGrid(Vector3 position, Vector3 rotation)
    {
        GameObject grid = Instantiate(GridPrefab, transform.position + position, Quaternion.Euler(rotation));
        gridsList.Add(grid);
    }

    public void DeleteOldGrid()
    {
        Object.Destroy(gridsList[0].gameObject);
        for (int i = 0; i < gridsList.Count-1; i++)
        {
            gridsList[i] = gridsList[i + 1];
        }
    }
}
