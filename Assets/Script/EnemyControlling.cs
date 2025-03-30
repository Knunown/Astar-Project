using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
    
    void Awake()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        player = PlayerStats.Instance;
        spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
        enemies = spawner.enemies;
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

    //public List<Node> EnemyFindPath()
    //{
    //    foreach(GameObject e in enemies)
    //    {
    //        enemy = e.GetComponent<Enemy>();
    //        Node startPoint = gridManager.NodeFromWorldPoint(e.transform.position);
    //        List<Node> movingPath = new List<Node>();
    //        movingPath.Add(startPoint);
    //        for(int i = 0; i < enemy.GetMovementRange() ; i++)
    //        {
    //            Node node = movingPath[i];
    //            List<Node> neighbors = gridManager.GetNeighbours(node);
    //            int random = Random.Range(1, neighbors.Count);
    //            movingPath.Add(neighbors[random]);
    //        }
    //        movingPath.RemoveAt(0);
    //        startPoint.SetStateNormalTile();
    //        movingPath[movingPath.Count].SetStateEnemyOn(enemy.GetIndex());



    //        return movingPath;
    //    }
    //}

    
}
