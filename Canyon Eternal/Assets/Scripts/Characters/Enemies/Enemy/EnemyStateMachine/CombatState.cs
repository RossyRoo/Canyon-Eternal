using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public AttackState attackState;
    public PursueState pursueTargetState;
    public DeathState deathState;
    public StunnedState stunnedState;
    public EvadeState evadeState;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        #region Handle State Switching

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        if (enemyManager.distanceFromTarget > enemyStats.attackRange)
        {
            return pursueTargetState;
        }

        if(enemyManager.distanceFromTarget < enemyStats.evadeRange)
        {
            //WE ONLY WANT TO GO TO THIS IF THE PLAYER IS TOO CLOSE FOR TOO LONG
            return evadeState;
        }

        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyStats.attackRange)
        {
            return attackState;
        }

        return this;
        #endregion


    }

}