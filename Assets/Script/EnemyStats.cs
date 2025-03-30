using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats" , menuName = "ScriptableObject/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Comabat Stats")]
    [SerializeField] private int damage;
    [SerializeField] private int stuntTime;
    [SerializeField] private bool triggered;

    [Header("Movement Stats")]
    [SerializeField] private int movementRange;
    [Header("Health Stats")]
    [SerializeField] private int health;

    [Header("Type Of Enenmy")]
    [SerializeField] private string enemyName;
    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int dmg)
    {
        health = Mathf.Max(0, health - dmg);
    }

    public void SetDamage(int dmg)
    {

    }

    public int GetDamage()
    {
        return damage;
    }

    public string GetName()
    {
        return enemyName;
    }

    public int GetRange()
    {
        return movementRange;
    }
}
