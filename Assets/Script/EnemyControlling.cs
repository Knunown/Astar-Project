using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyControlling : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] GridManager gridManager;
    [SerializeField] private GameObject enemyPrefab;
    private PlayerStats player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Enemy enemy;

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<Enemy> enemiesScript = new List<Enemy>();
    [SerializeField] private CharacterControlling characterControlling;

    
    void Awake()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        player = PlayerStats.Instance;
        spawner = Spawner.Instance;
        enemies = spawner.GetEnemies();
        characterControlling = gameObject.GetComponent<CharacterControlling>();
        //enemiesNode = spawner.enemies;
    }

    private void Start()
    {
        enemiesScript = spawner.enemies;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EnemyRandomlyMoving());
        }
    }

    public void BeDamaged(int enemyIndex, int dmg)
    {
        // Execute from Enemy, enemyIndex refered from Spawner
        enemy = enemies[enemyIndex].GetComponent<Enemy>();
        enemy.BeDamaged(dmg);
        Debug.Log("Dodamage");
    }


    public Enemy GetEnemy(int enemyIndex)
    {
        return enemies[enemyIndex].GetComponent<Enemy>(); ;
    }
    public int getHealth(int enemyIndex)
    {
        enemy = enemies[enemyIndex].GetComponent<Enemy>();
        return enemy.GetHealth();
    }

    public int getCurrentHealth(int enemyIndex)
    {
        enemy = enemies[enemyIndex].GetComponent<Enemy>();
        return enemy.GetCurrentHealth();
    }

    public int getDamage(int enemyIndex)
    {
        enemy = enemies[enemyIndex].GetComponent<Enemy>();
        return enemy.GetDamage();
    }

    public int GetMovementRange(int enemyIndex)
    {
        enemy = enemies[enemyIndex].GetComponent<Enemy>();
        return enemy.GetMovementRange();
    }


    public HashSet<Node> GetEnemyRange(Node node, int range)
    {
        HashSet<Node> seenNodes = new HashSet<Node>();
        Queue<Node> currentNodes = new Queue<Node>();

        // Initialize with the starting node
        currentNodes.Enqueue(node);
        seenNodes.Add(node);

        for (int i = 0; i < range; i++)
        {
            int count = currentNodes.Count;

            for (int j = 0; j < count; j++)
            {
                Node currentNode = currentNodes.Dequeue();
                List<Node> neighbors = gridManager.GetNeighbours(currentNode);

                foreach (Node neighbor in neighbors)
                {
                    if (!seenNodes.Contains(neighbor) && neighbor.walkable)
                    {
                        seenNodes.Add(neighbor);
                        currentNodes.Enqueue(neighbor);
                    }
                }
            }
        }

        seenNodes.Remove(node);
        return seenNodes;
    }

    //public void EnemyMoving()
    //{
    //    EnemyFindPath();
    //    foreach (Node n in path)
    //    {
    //        Vector3 directionToTarget = n.worldPosition - player.transform.position;
    //        Vector3 objectForward = player.transform.forward;
    //        float angle = Vector3.Angle(objectForward, directionToTarget);
    //        Vector3 rotationAxis = Vector3.Cross(objectForward, directionToTarget);
    //        if (rotationAxis == Vector3.zero)
    //        {
    //            rotationAxis = Vector3.Cross(player.transform.right, directionToTarget);
    //        }
    //        Quaternion angle2 = Quaternion.AngleAxis(angle, rotationAxis) * player.transform.rotation;

    //        float timer = 0f;
    //        Vector3 currentPosition = player.transform.position;
    //        animator.SetBool("Moving", true);
    //        while (timer < jumpDuration)
    //        {
    //            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, angle2, angle / (jumpDuration / 2) * Time.deltaTime);
    //            player.transform.position = Vector3.Lerp(currentPosition, n.worldPosition, timer / jumpDuration);
    //            timer += Time.deltaTime;
    //            yield return null;
    //        }
    //        animator.SetBool("Moving", false);
    //        yield return new WaitForSeconds(0.1f);
    //        player.transform.position = n.worldPosition;
    //    }
    //}



    private IEnumerator EnemyRandomlyMoving()
    {
        int stepCount = -1;

        //--consider to remove--
        Dictionary<Enemy, Node> path = new Dictionary<Enemy, Node>();

        List<Node> unwalkableNodes = new List<Node>();
        //----------------------

        int numberOfEnemies = enemies.Count;


        while (numberOfEnemies > 0)
        {
            stepCount++;

            //--consider to remove--
            unwalkableNodes.Clear();
            //----------------------

            foreach (Enemy e in enemiesScript)
            {
                int randomNode;
                Node finishNode;
                if (stepCount != e.GetMovementRange())
                {
                    List<Node> neighbors = gridManager.GetNeighbours(e.GetEnemyNode());
                    gridManager.grid[e.GetEnemyNode().gridX,e.GetEnemyNode().gridY].SetStateNormalTile();

                    neighbors = neighbors.FindAll(n => !unwalkableNodes.Contains(n) && n.walkable);

                    randomNode = UnityEngine.Random.Range(1, neighbors.Count);
                    finishNode = neighbors[randomNode];
                    e.SetEnemyNode(finishNode);

                    gridManager.grid[finishNode.gridX, finishNode.gridY] = e.GetEnemyNode();

                    Vector3 nextStep = finishNode.worldPosition;
                    
                    //--consider to remove--
                    unwalkableNodes.Add(finishNode);
                    //----------------------

                    e.gameObject.transform.position = nextStep;
                    //characterControlling.StartCoroutine(Moving());
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    numberOfEnemies--;
                }
            }
        }
    }

    
}
