using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public PursueState pursueTargetState;
    public DeathState deathState;

    public LayerMask detectionLayer;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyManager.isDead)
        {
            return deathState;
        }

        #region Handle Target Detection
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), enemyStats.detectionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerStats playerStats = colliders[i].GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                enemyManager.currentTarget = playerStats;
                Debug.Log("Found Player");
            }
        }
        #endregion

        #region Handle Switch State

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }

        #endregion

    }
}
