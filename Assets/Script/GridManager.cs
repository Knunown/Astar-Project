using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{



    [SerializeField] private Spawner spawner;
    [SerializeField] private EnemyControlling enemyControlling;

    public GameObject playerCharacter;
    public GameObject doorWay;
    public GameObject wall;
    public GameObject[] tilesToSpawn; 
    public GameObject[] gameObjectsToSpawn;
    private Vector3[] spawnPositions;
    public Vector3 tilesScale = new Vector3(1,1,1);

    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius = 0.5f;


    //Very important define the hole game
    public Node[,] grid;
    

    private float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
        enemyControlling = GameObject.Find("InteractionManager").GetComponent<EnemyControlling>();
    }

    private void Start()
    {
        SpawnTilesOnPlane();
        SpawnPlayer();
        SpawnDoor();
        SpawnWall();
        SpawnEnemy();
    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }


    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / gridWorldSize.x + 0.5f;
        float percentY = worldPosition.z / gridWorldSize.y + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    public List<Node> path;


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1, .1f, 1) * (nodeDiameter - .1f));

            }
        }
    }



    void SpawnTilesOnPlane()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                GameObject randomObject = tilesToSpawn[UnityEngine.Random.Range(0, tilesToSpawn.Length)];
                GameObject spawnedObject = Instantiate(randomObject, n.worldPosition, Quaternion.identity);
                spawnedObject.name = randomObject.name+"[" + n.gridX + "," + n.gridY + "]";
                n.tileName = spawnedObject.name;
                spawnedObject.transform.localScale = tilesScale;
            }
        }
    }

    void SpawnGameObjectOnPlane()
    {
    }



    void SpawnPlayer()
    {
        if(grid != null)
        {
            int zSpawn = gridSizeY/2;
            int xSpawn = gridSizeX/2;
            GameObject spawnedPlayer = Instantiate(playerCharacter, grid[xSpawn,0].worldPosition, Quaternion.identity);
            spawnedPlayer.transform.localScale = new Vector3(1,1,1);
            grid[xSpawn, 0].walkable = false;
            grid[xSpawn, 0].havePlayerOn = true;
        }
    }


    void SpawnEnemy()
    {
        //Called from Spawner
        spawner.SpawnEnemy();
    }


    void SpawnDoor()
    {
        if (grid != null)
        {
            Vector3 spawnPosition = new Vector3(0, 0 , gridWorldSize.y/2);
            GameObject spawnedDoor = Instantiate(doorWay, spawnPosition, Quaternion.identity);
            spawnedDoor.transform.localScale = new Vector3(1, 1, 1);
            for(int i = 0; i < (gridWorldSize.x - 3)/2; i +=2)
            {
                spawnPosition = new Vector3(-(gridWorldSize.x / 2) + i, 0, (gridWorldSize.y / 2));
                GameObject spawnedWall = Instantiate(wall, spawnPosition, Quaternion.identity);
                spawnedWall = Instantiate(wall, spawnPosition + new Vector3(gridWorldSize.x + i * (-2) - 2,0,0), Quaternion.identity);
                spawnedWall.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    void SpawnWall()
    {
        if (grid != null)
        {
            for (int i = 0; i < gridWorldSize.y; i += 2)
            {
                Vector3 spawnPosition = new Vector3(-(gridWorldSize.x / 2), 0, -(gridWorldSize.y / 2) + i);
                GameObject spawnedWall = Instantiate(wall, spawnPosition, Quaternion.Euler(0, -90, 0));
                spawnedWall = Instantiate(wall, spawnPosition * (-1), Quaternion.Euler(0, 90, 0));
                spawnedWall.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    public void SetGridNode(Node node)
    {
        grid[node.gridX, node.gridY] = node;
    }
}
