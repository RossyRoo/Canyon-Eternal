﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public AttackState attackState;
    public PursueState pursueTargetState;
    public DeathState deathState;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.isDead)
        {
            return deathState;
        }
        
        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        else if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

}