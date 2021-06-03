using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public CombatState combatStanceState;
    public DeathState deathState;
    public StunnedState stunnedState;
    public FleeState fleeState;

    public EnemyAttackAction currentAttack;

    [Tooltip("Use this until you add acceleration and deceleration to attacks")]
    private float chargeForceMultiplier = 5000f;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.isPerformingAction)
            return combatStanceState;

        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.spaceNeededToStartAttack)
            {
                return fleeState;
            }
            else if (distanceFromTarget < currentAttack.shortestDistanceNeededToAttack)
            {
                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                {
                    PerformAttack(enemyManager, enemyAnimatorHandler, targetDirection);

                    return combatStanceState;
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        #region Handle State Switching

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        return combatStanceState;

        #endregion

    }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyManager.enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyManager.enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.shortestDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.spaceNeededToStartAttack)
            {
                maxScore += enemyAttackAction.attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyManager.enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyManager.enemyAttacks[i];

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

    private void PerformAttack(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler, Vector2 targetDirection)
    {
        enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.isPerformingAction = true;

        enemyAnimatorHandler.UpdateFloatAnimationValues(targetDirection.normalized.x, targetDirection.normalized.y, false);

        if (currentAttack.chargeForce != 999f)
        {
            //APPLY ACCELERATION
            Vector2 force = (targetDirection.normalized * currentAttack.chargeForce * Time.deltaTime) * chargeForceMultiplier;
            enemyManager.rb.AddForce(force);
        }

        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

        currentAttack = null;
    }

} 