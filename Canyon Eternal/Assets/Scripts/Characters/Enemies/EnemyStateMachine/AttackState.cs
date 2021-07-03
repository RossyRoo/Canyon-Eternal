using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    public PursueState pursueState;

    EnemyAttackAction currentAttack;

    [Tooltip("Use this until you add acceleration and deceleration to attacks")]
    private float chargeForceMultiplier = 5000f;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        GetNewAttack(enemyManager, enemyStats);

        if (currentAttack != null)
        {
            PerformAttack(enemyManager, enemyStats, enemyAnimatorHandler);
            return pursueState;
        }

        return pursueState;

    }

    private void GetNewAttack(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyStats.characterData.enemyAttacks.Length; i++)
        {
            if (distanceFromTarget <= enemyStats.characterData.enemyAttacks[i].shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyStats.characterData.enemyAttacks[i].spaceNeededToStartAttack)
            {
                maxScore += enemyStats.characterData.enemyAttacks[i].attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyStats.characterData.enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyStats.characterData.enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.spaceNeededToStartAttack)
            {
                if (currentAttack != null)
                    return;

                temporaryScore += enemyAttackAction.attackScore;

                if (temporaryScore > randomValue)
                {
                    currentAttack = enemyAttackAction;
                }
            }
        }

    }

    private void PerformAttack(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if(currentAttack != null)
        {
            Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorHandler.UpdateFloatAnimationValues(targetDirection.normalized.x, targetDirection.normalized.y, false);

            StartCoroutine(enemyStats.HandleAttackDamageColliders(currentAttack));
            StartCoroutine(enemyStats.HandleBlockVulnerability(currentAttack));

            if (currentAttack.chargeForce != 999f)
            {
                Vector2 force = (targetDirection.normalized * currentAttack.chargeForce * Time.deltaTime) * chargeForceMultiplier;
                enemyManager.rb.AddForce(force);
            }

            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

    }

} 