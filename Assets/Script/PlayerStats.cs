using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
    private static PlayerStats instance;

    public static PlayerStats Instance { get => instance; }

    [Header("Comabat Stats")]
    [SerializeField] private int damage = 15;
    [SerializeField] private int stuntTime;           

    [Header("Movement Stats")]

    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentHealth = maxHealth;
    }

    public void BeDamaged(int dmg)
    {
        currentHealth = Mathf.Max(0, currentHealth - dmg);
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
