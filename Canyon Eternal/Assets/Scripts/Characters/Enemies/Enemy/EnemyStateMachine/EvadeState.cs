using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : EnemyStateMachine
{
    //We go to this state if the player gets too close to the enemy for too long, or if the enemy's health gets low and they can use heal items
    //We want the behavior to get the proper flee distance by hopping back, then determining whether its time to heal or return to combat state

    public CombatState combatState;
    public float leapForceMultiplier = 200;

    public float safeDistanceBuffer = 5f; //Add this to the attack range to determine distance enemy needs to start their evade recovery timer
    public float evadeRecoveryTime = 0.5f;
    public float evadeRecoveryStartTime = 0.5f;
    public bool reachedEvadeDistance = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        Vector2 force = (-targetDirection.normalized * 50f * Time.deltaTime) * leapForceMultiplier;

        if(reachedEvadeDistance)
        {
            evadeRecoveryTime -= Time.deltaTime;
        }

        if (enemyManager.distanceFromTarget > enemyManager.attackRange + safeDistanceBuffer)
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
            //Play leap back animation and update animator
            enemyManager.rb.AddForce(force);
        }
        return this;
    }

}
