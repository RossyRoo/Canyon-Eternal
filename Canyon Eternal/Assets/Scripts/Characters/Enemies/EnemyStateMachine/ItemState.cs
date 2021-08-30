using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : EnemyStateMachine
{
    public PursueState pursueState;

    public List<Consumable> consumableStock;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {

        UseHealItem(enemyStats);
        return pursueState;


    }

    private void UseHealItem(EnemyStats enemyStats)
    {
        enemyStats.currentHealth += consumableStock[0].healAmount;
        consumableStock.Remove(consumableStock[0]);
        //Play that consumables heal VFX
    }
}
