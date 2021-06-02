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
            Debug.Log("Stun Time Over");
            StartCoroutine(StunTimer(enemyManager));
        }

        if(finishedStunnedCoroutine)
        {
            finishedStunnedCoroutine = false;
            runningStunnedCoroutine = false;

            for (int i = 0; i < enemyManager.myDamageColliders.Length; i++)
            {
                enemyManager.myDamageColliders[i].EnableDamageCollider();
            }

            return pursueState;
        }
        else
        {
            return this;
        }
    }

    private IEnumerator StunTimer(EnemyManager enemyManager)
    {
        runningStunnedCoroutine = true;

        for (int i = 0; i < enemyManager.myDamageColliders.Length; i++)
        {
            enemyManager.myDamageColliders[i].DisableDamageCollider();
        }

        yield return new WaitForSeconds(enemyManager.stunTime);

        finishedStunnedCoroutine = true;

        Debug.Log("Stun Time Over");
    }
}
