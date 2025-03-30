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
    }

    public void BeDamaged(int dmg)
    {
        this.currentHealth = Mathf.Max(0 , currentHealth - dmg);
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

    public Node GetCord()
    {
        return gridManager.NodeFromWorldPoint(transform.position);
    }

    public void SetEnemyNode(Node n)
    {
        n.haveEnemyOn = true;
        n.walkable = false;
        n.enemyIndex = this.enemyIndex;
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
