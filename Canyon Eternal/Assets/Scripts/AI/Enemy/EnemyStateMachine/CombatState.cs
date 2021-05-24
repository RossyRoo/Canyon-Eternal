using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : EnemyStateMachine
{
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

        HandleRotateTowardsTarget(enemyManager);

        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
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

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {

    }
}