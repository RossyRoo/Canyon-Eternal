using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : EnemyStateMachine
{
    public CombatState combatState;

    float safeDistanceBuffer = 2f;
    float evadeRecoveryTime = 0.5f;
    float evadeRecoveryStartTime = 0.5f;
    bool reachedEvadeDistance = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        Vector2 force = (-targetDirection.normalized * Time.deltaTime) * enemyStats.characterData.moveSpeed;


        if (enemyManager.distanceFromTarget > enemyStats.characterData.attackRange + safeDistanceBuffer || reachedEvadeDistance)
        {
            reachedEvadeDistance = true;
        }
        else
        {
            enemyManager.rb.AddForce(force);
        }

        if (reachedEvadeDistance)
        {
            evadeRecoveryTime -= Time.deltaTime;
        }

        if (evadeRecoveryTime < 0)
        {
            evadeRecoveryTime = evadeRecoveryStartTime;
            reachedEvadeDistance = false;
            return combatState;
        }



        return this;
    }

}
