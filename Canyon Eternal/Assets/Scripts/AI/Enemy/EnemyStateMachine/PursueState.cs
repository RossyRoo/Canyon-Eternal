using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : EnemyStateMachine
{

    public CombatState combatStanceState;
    public DeathState deathState;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        return this;
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        
    }
}