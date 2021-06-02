using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public CombatState combatStanceState;
    public DeathState deathState;
    public StunnedState stunnedState;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;


    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region Handle Death and Stun States
        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }
        #endregion

        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        //float viewableAngle = Vector2.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        
        if (enemyManager.isPerformingAction)
            return combatStanceState;

        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
            {
                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                {
                    enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                    enemyManager.isPerformingAction = true;
                    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

                    if (currentAttack.chargeForce != 999f) //CHARGE ATTACKS ONLY
                    {
                        Vector2 force = targetDirection * currentAttack.chargeForce * Time.deltaTime;
                        enemyManager.rb.AddForce(force);
                    }
                    currentAttack = null;

                    return combatStanceState;
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;

        }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                maxScore += enemyAttackAction.attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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


}



    
       