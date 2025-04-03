using UnityEngine;


public enum BattleState { PLAYERTURN , PlAYERSELECTED , ENEMYSELECTED , ATTACKINGBEHAVIOR , MOVINGBEHAVIOR , PLAYERATTACKING , PLAYERMOVING , ENEMYTURN , ENEMYATTACKING}
public class BattleSystem : MonoBehaviour
{
    private BattleState state;

    Spawner spawner;
    private void Awake()
    {
        spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
    }
    private void Update()
    {
        if(state == BattleState.ENEMYTURN)
        {
            EnemyBehavior();
        }
    }

    private void EnemyBehavior()
    {
    }
}
