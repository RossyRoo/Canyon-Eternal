using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : EnemyStateMachine
{
    public PursueState pursueState;

    float evadeSpeedMultiplier = 1.5f;
    float safeDistanceBuffer = 10f;
    float evadeRecoveryTime = 0.5f;
    float evadeRecoveryStartTime = 0.5f;
    bool reachedEvadeDistance = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        Vector2 force = (-targetDirection.normalized * Time.deltaTime) * (enemyStats.characterData.moveSpeed * evadeSpeedMultiplier);

        //UPDATE ANIMATIONS SO YOU FACE TOWARD PLAYER AND DO DASH ANIMATION

        if (!reachedEvadeDistance &&
            enemyManager.distanceFromTarget > safeDistanceBuffer)
        {
            reachedEvadeDistance = true;
        }

        if(!reachedEvadeDistance)
        {
            enemyManager.rb.AddForce(force);
        }
        else
        {
            evadeRecoveryTime -= Time.deltaTime;
        }

        if (evadeRecoveryTime < 0)
        {
            evadeRecoveryTime = evadeRecoveryStartTime;
            reachedEvadeDistance = false;
            return pursueState;
        }

        return this;
    }

}
