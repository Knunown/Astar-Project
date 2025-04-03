using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner instance;

    public static Spawner Instance { get => instance; }
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private List<GameObject> enemiesGO = new List<GameObject>();
    [SerializeField] private int numberOfEnemies;
    Node[,] nodes;
    //[SerializeField] public Dictionary<Enemy, Node> enemies = new Dictionary<Enemy, Node>();
    [SerializeField] public List<Enemy> enemies = new List<Enemy>();
    [SerializeField] GridManager gridManager;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
    }   
    

    public void SpawnEnemy()
    {
        if (gridManager.grid != null)
        {
            foreach (Node n in gridManager.grid)
            {
                int random = Random.Range(1, 20);
                if (n.walkable && numberOfEnemies > 0 && random == 1)
                {
                    //Generate new enemies and attach value to them (index, name)
                    GameObject randomEnemy = enemy[Random.Range(0, enemy.Length)];
                    GameObject newEnemy = Instantiate(randomEnemy, n.worldPosition, Quaternion.identity);
                    newEnemy.transform.localScale = new Vector3(1, 1, 1);
                    Enemy enemyScript = newEnemy.GetComponent<Enemy>();

                    enemiesGO.Add(newEnemy);
                    //Set the index of the enemy
                    enemyScript.SetIndex(enemiesGO.IndexOf(newEnemy));

                    //Name the enemy
                    newEnemy.name = enemyScript.GetName();
                    Debug.Log("Enemy Spawned");
                    
                    //Set value the node of the grid that have enemy on (haveEnemyOn, walkable, enemyIndex)
                    enemyScript.SetEnemyNode(n);
                    gridManager.SetGridNode(enemyScript.GetEnemyNode());
                    n.enemyIndex = enemyScript.GetIndex();

                    enemies.Add(enemyScript);
                    numberOfEnemies--;
                }
                else if(numberOfEnemies == 0)
                {
                    break;
                }
            }
        }
    }

    public List<GameObject> GetEnemies()
    {
        return enemiesGO;
    }

}
