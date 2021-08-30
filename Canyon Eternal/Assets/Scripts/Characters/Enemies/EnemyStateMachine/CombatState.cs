using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public AttackState attackState;
    public PursueState pursueTargetState;
    public DeathState deathState;
    public StunnedState stunnedState;
    public EvadeState evadeState;
    public ItemState itemState;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        #region Handle State Switching

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        //ADD RANDOM CHANCE TO THIS
        if(itemState != null
            && itemState.consumableStock.Count > 0
            && enemyStats.currentHealth <= (enemyStats.characterData.startingMaxHealth / 3))
        {
            Debug.Log("Using Item");
            return itemState;
        }

        if (enemyManager.currentRecoveryTime <= 0
            && enemyManager.distanceFromTarget <= enemyStats.characterData.attackRange
            && enemyStats.characterData.canAttack
            && !enemyManager.isInteracting)
        {
            return attackState;
        }

        if (enemyManager.distanceFromTarget > enemyStats.characterData.attackRange
            && enemyStats.characterData.canPursue)
        {
            return pursueTargetState;
        }

        if(enemyManager.distanceFromTarget < enemyStats.characterData.evadeRange && enemyStats.characterData.canEvade && !enemyManager.currentTarget.GetComponent<PlayerManager>().isDashing)
        {
            if (Random.value > 0.1) //percent chance they will evade
            {
                return evadeState;
            }
        }

        return this;
        #endregion


    }

}