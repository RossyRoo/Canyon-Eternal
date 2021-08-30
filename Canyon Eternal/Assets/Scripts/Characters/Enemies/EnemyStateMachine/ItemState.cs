using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : EnemyStateMachine
{
    public PursueState pursueState;
    public List<Consumable> myConsumables;
    public bool allConsumablesUsed;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        

        if(myConsumables.Count > 0)
        {
            UseHealItem(enemyStats);
        }
        else
        {
            allConsumablesUsed = true;
        }

        return pursueState;

    }

    private void UseHealItem(EnemyStats enemyStats)
    {
        enemyStats.currentHealth += (myConsumables[0].healAmount * 100f);

        if(enemyStats.currentHealth > enemyStats.characterData.startingMaxHealth)
        {
            enemyStats.currentHealth = enemyStats.characterData.startingMaxHealth;
        }

        enemyStats.GetComponentInChildren<EnemyHealthBarUI>().DisplayHealthBar();

        if(myConsumables[0].useVFX != null)
        {
            GameObject useVFX = Instantiate(myConsumables[0].useVFX, transform.position, Quaternion.identity);
            useVFX.transform.SetParent(FindObjectOfType<ObjectPool>().transform);
            Destroy(useVFX, 2f);
        }

        myConsumables.Remove(myConsumables[0]);
        //Run use item animation and wait for it to complete

    }
}
