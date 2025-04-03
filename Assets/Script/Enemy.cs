using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyIndex;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private int health;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int movementRange;
    [SerializeField] private int triggered;

    [SerializeField] private Node node;
    [SerializeField] GridManager gridManager;
    PlayerStats player;

    private void Start()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        health = enemyStats.GetHealth();
        currentHealth = health;
        damage = enemyStats.GetDamage();
        movementRange = enemyStats.GetRange();
        player = PlayerStats.Instance;
        node.SetStateEnemy();
    }

    public void BeDamaged(int dmg)
    {
        this.currentHealth = Mathf.Max(0 , currentHealth - dmg);
        if(this.currentHealth==0)
        {
            Destroy(gameObject);
        }
    }

    public void DoDamage(int dmg)
    {
        player.BeDamaged(damage);
    }

    public int GetIndex()
    {
        return this.enemyIndex;
    }

    public void SetIndex(int index)
    {
        this.enemyIndex = index;
    }

    public string GetName()
    {
        return enemyStats.GetName() + this.enemyIndex;
    }

    public void SetGridNode(Node node)
    {
        gridManager.grid[this.node.gridX,this.node.gridY] = node; 
    }   

    public Node GetEnemyNode()
    {
        return this.node;
    }


    public void SetEnemyNode(Node node)
    {
        this.node = node;
        this.node.SetStateEnemy();
    }


    public int GetHealth()
    {
        return health;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetMovementRange()
    {
        return movementRange;
    }

    
}
