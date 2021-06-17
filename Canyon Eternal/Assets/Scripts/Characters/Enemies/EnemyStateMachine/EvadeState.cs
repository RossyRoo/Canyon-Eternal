using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : EnemyStateMachine
{
    //We go to this state if the player gets too close to the enemy for too long, or if the enemy's health gets low and they can use heal items
    //We want the behavior to get the proper flee distance by hopping back, then determining whether its time to heal or return to combat state

    public CombatState combatState;
    float leapForceMultiplier = 200f;

    float safeDistanceBuffer = 2f;
    float evadeRecoveryTime = 0.5f;
    float evadeRecoveryStartTime = 0.5f;
    bool reachedEvadeDistance = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        Vector2 force = (-targetDirection.normalized * 50f * Time.deltaTime) * leapForceMultiplier;

        if(reachedEvadeDistance)
        {
            evadeRecoveryTime -= Time.deltaTime;
        }

        if (enemyManager.distanceFromTarget > enemyStats.characterData.attackRange + safeDistanceBuffer)
        {
            reachedEvadeDistance = true;

            if (evadeRecoveryTime < 0)
            {
                evadeRecoveryTime = evadeRecoveryStartTime;
                reachedEvadeDistance = false;
                return combatState;
            }
        }
        else
        {
            enemyManager.rb.AddForce(force);
        }
        return this;
    }

}
