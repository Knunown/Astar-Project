using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemy;
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int numberOfEnemies;


    [SerializeField] GridManager gridManager;
    private void Awake()
    {
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
                    Enemy enemyScript = newEnemy.GetComponentInChildren<Enemy>();
                    enemies.Add(newEnemy);
                    //Set the index of the enemy
                    enemyScript.SetIndex(enemies.IndexOf(newEnemy));
                    newEnemy.name = enemyScript.GetName();
                    Debug.Log("Enemy Spawned");
                    numberOfEnemies--;

                    //Set value the node of the grid that have enemy on (haveEnemyOn, walkable, enemyIndex)
                    n.haveEnemyOn = true;
                    n.walkable = false;
                    n.enemyIndex = enemyScript.GetIndex();
                }
                else if(numberOfEnemies == 0)
                {
                    break;
                }
            }
        }
    }


}
