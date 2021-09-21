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
            FindObjectOfType<ConsumableHandler>().HandleEnemyConsumable(myConsumables[0], enemyStats);
        }
        else
        {
            allConsumablesUsed = true;
        }

        return pursueState;

    }

}
