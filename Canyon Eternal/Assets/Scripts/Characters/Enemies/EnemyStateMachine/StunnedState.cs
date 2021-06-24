using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : EnemyStateMachine
{
    public DeathState deathState;
    public PursueState pursueState;

    bool runningStunnedCoroutine;
    bool finishedStunnedCoroutine;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region Handle Death State

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

            enemyAnimatorHandler.PlayTargetAnimation("Idle", false);
            enemyManager.isStunned = false;

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

        enemyAnimatorHandler.PlayTargetAnimation("Stunned", false);

        enemyStats.DisableAllDamageColliders();

        yield return new WaitForSeconds(1f);

        finishedStunnedCoroutine = true;
    }
}
