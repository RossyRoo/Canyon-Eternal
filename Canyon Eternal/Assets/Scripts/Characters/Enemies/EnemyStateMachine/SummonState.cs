using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonState : EnemyStateMachine
{
    public PursueState pursueState;

    public GameObject[] summons;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        
        return pursueState;

    }
}
