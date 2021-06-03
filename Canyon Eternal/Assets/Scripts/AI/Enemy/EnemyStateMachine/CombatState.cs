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

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        #region Handle State Switching

        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }

        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyManager.isMoving = true;
            return pursueTargetState;
        }

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        return this;
        #endregion


    }

}