using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyStatsDisplay;
    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private TMP_Text enemyHealth;
    [SerializeField] private TMP_Text enemyDamage;
    public void DisplayEnemyStats(Enemy e)
    {
        enemyName.text = e.GetName();
        enemyHealth.text = e.GetCurrentHealth().ToString() + "/" + e.GetHealth();
        enemyDamage.text = e.GetDamage().ToString();
        enemyStatsDisplay.SetActive(true);
    }
}
