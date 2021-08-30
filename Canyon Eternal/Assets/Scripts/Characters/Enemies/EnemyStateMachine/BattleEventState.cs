using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventState : EnemyStateMachine
{
    public PursueState pursueState;

    public bool fleesToSafeZone;
    public string eventAnimation;
    public float eventTime;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        //Find the "safe zone" and jump to it
        //Play animation
        //Start timer
        //After timer is finished, jump back to starting point
        
        return pursueState;

    }
}
