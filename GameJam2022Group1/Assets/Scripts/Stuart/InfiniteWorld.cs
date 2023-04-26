using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace StuartH
{
    /// <summary>
    ///InfiniteWorld - Handles creation and destruction of infinite world.
    /// </summary>
    public class InfiniteWorld : MonoBehaviour
    {

        [SerializeField] private List<GameObject> straightPieces;
        [SerializeField] private List<GameObject> leftPieces;
        [SerializeField] private List<GameObject> rightPieces;
       [Range(0, 20)] [SerializeField] private int maxSpawnedPieces = 10;
        [Range(2, 12)] [SerializeField] private int startPieces = 4;
        [Range(2, 10)] [SerializeField] private int startStraightPieces = 4;

        [Range(0, 20)] [SerializeField] private int tileSize = 5;
        [Range(0f, 1f)] [SerializeField] private float cornerChance = 0.1f;
        [Range(0f, 1f)] [SerializeField] private float minCornerAmount = 0.15f;
        [Range(0f, 1f)] [SerializeField] private float maxCornerAmount = 0.9f;

        private Queue<GameObject> spawnedPieces = new Queue<GameObject>();
        private Queue<PieceType> spawnedPieceTypes = new Queue<PieceType>();

        private Compass compass = Compass.North;
        private Vector3 currentPosition = new Vector3(0, 0, 0);
        private Vector3 currentDirection;
        private List<Vector3> directions = new List<Vector3>();

        
        
        public void SetCornerRate(float v) => cornerChance = v;

        public float GetMinCornerAmount() => minCornerAmount;

        public float GetMaxCornerAmount() => maxCornerAmount;
        protected void Awake()
        {
            CreateDirections();
            currentDirection = directions[(int)compass];
            

            SpawnNext(straightPieces[0]);
            SpawnNext(straightPieces[0]);

            for (var i = 0; i < startStraightPieces - 2; i++)
            {
                SpawnNext(straightPieces[Random.Range(0, straightPieces.Count)]);
            }
            SpawnNext(leftPieces[Random.Range(0, leftPieces.Count)]);
            for (var i = 0; i < startPieces - startStraightPieces - 1; i++)
            {
                SpawnNext();
            }
        }

        private void CreateDirections()
        {
            directions.Add(new Vector3(0, 0, tileSize)); //n
            directions.Add(new Vector3(tileSize, 0, 0)); //e
            directions.Add(new Vector3(0, 0, -tileSize)); //s
            directions.Add(new Vector3(-tileSize, 0, 0)); //w
        }


        private void SpawnNext(GameObject requestedPrefab = null)
        {
            while (true)
            {
                GameObject spawnedObject = null;
                if (requestedPrefab == null) requestedPrefab = DetermineNext();
                PieceType pieceType;
                
                if (straightPieces.Contains(requestedPrefab)) pieceType = PieceType.Straight;
                else if (leftPieces.Contains(requestedPrefab)) pieceType = PieceType.Left;
                else pieceType = PieceType.Right;
                
                var spawnedPieceTypesArr = spawnedPieceTypes.ToArray();
                if (pieceType == PieceType.Left || pieceType == PieceType.Right)
                {
                    if (spawnedPieceTypesArr.Length > 2 &&
                        (spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 1] == pieceType &&
                         spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 2] == pieceType))
                    {
                        requestedPrefab = null;
                        continue;
                    }
                }
                CalculateDirection(pieceType);
                SpawnPiece(requestedPrefab, pieceType);
                break;
                
            }
        }
        

        private void SpawnPiece(GameObject requestedPrefab, PieceType pieceType)
        {
            GameObject spawnedObject;
            spawnedPieceTypes.Enqueue(pieceType);
            spawnedObject = Instantiate(requestedPrefab, currentPosition, Quaternion.Euler(0, 90 * (int)compass, 0),
                transform);
            spawnedObject.GetComponent<Tile>().Init(DeleteCallback);
            currentPosition += currentDirection;
            spawnedPieces.Enqueue(spawnedObject);
        }

        private void CalculateDirection(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Left:
                {
                    compass -= 1;
                    if (compass < Compass.North) compass = Compass.West;
                    break;
                }
                case PieceType.Right:
                {
                    compass += 1;
                    if (compass > Compass.West) compass = Compass.North;
                    break;
                }
            }

            currentDirection = directions[(int)compass];
        }

        private bool isFirst = true;
        private void DeleteCallback(GameObject tile)
        {
            if (isFirst)
            {
                isFirst = false;
                SpawnNext();
                return;
            }
            Destroy(spawnedPieces.Dequeue());
            SpawnNext();
        }

        private GameObject DetermineNext()
        {
            var spawnedPieceTypesArr = spawnedPieceTypes.ToArray();
            if (spawnedPieceTypesArr.Length > 2 &&
                (spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 1] == PieceType.Left &&
                 spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 2] == PieceType.Left))
            {
                return Random.Range(0f,1f)>0.5f ? rightPieces[Random.Range(0, rightPieces.Count)] : straightPieces[Random.Range(0, straightPieces.Count)];
            }
            if (spawnedPieceTypesArr.Length > 2 &&
                      (spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 1] == PieceType.Right &&
                       spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 2] == PieceType.Right))
            {
                return Random.Range(0f,1f)>0.5f ? leftPieces[Random.Range(0, leftPieces.Count)] : straightPieces[Random.Range(0, straightPieces.Count)];

            }
            if (spawnedPieceTypesArr.Length > 3 &&
                (spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 2] == PieceType.Left &&
                 spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 3] == PieceType.Left))
            {
                return Random.Range(0f,1f)>0.5f ? rightPieces[Random.Range(0, rightPieces.Count)] : straightPieces[Random.Range(0, straightPieces.Count)];
            }
            if (spawnedPieceTypesArr.Length > 3 &&
                (spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 2] == PieceType.Right &&
                 spawnedPieceTypesArr[spawnedPieceTypesArr.Length - 3] == PieceType.Right))
            {
                return Random.Range(0f,1f)>0.5f ? leftPieces[Random.Range(0, leftPieces.Count)] : straightPieces[Random.Range(0, straightPieces.Count)];
            }
            GameObject go = null;
            if (!(Random.Range(0f, 1f) > 1f - cornerChance)) return straightPieces[Random.Range(0, straightPieces.Count)];
            go = Random.Range(0f, 1f) > 0.5f ? leftPieces[Random.Range(0, leftPieces.Count)] : rightPieces[Random.Range(0, rightPieces.Count)];
            return go;
        }

        private enum Compass
        {
            North,
            East,
            South,
            West
        }

        private enum PieceType
        {
            Straight,
            Left,
            Right
        }


        public List<Transform> GetAllTiles()=>spawnedPieces.Select(s => s.transform).ToList();
        
    }
}
