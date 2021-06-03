using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : EnemyStateMachine
{
    public PursueState pursueState;
    public DeathState deathState;

    bool runningStunnedCoroutine;
    bool finishedStunnedCoroutine;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region Handle Death and Stun State
        if (enemyManager.isStunned)
        {
            enemyManager.isStunned = false;
        }

        if (enemyManager.isDead)
        {
            return deathState;
        }
        #endregion

        if (!runningStunnedCoroutine)
        {
            StartCoroutine(StunTimer(enemyStats, enemyAnimatorHandler));
        }

        if(finishedStunnedCoroutine)
        {
            finishedStunnedCoroutine = false;
            runningStunnedCoroutine = false;

            enemyStats.EnableAllDamageColliders();

            enemyManager.isInteracting = false;

            return pursueState;
        }
        else
        {
            return this;
        }
    }

    private IEnumerator StunTimer(EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        runningStunnedCoroutine = true;

        enemyAnimatorHandler.PlayTargetAnimation("Stunned", true);

        enemyStats.DisableAllDamageColliders();

        yield return new WaitForSeconds(2f);

        finishedStunnedCoroutine = true;
    }
}
