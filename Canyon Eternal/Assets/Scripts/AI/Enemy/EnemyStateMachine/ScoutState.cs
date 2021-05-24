using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutState : EnemyStateMachine
{
    public PursueState pursueTargetState;
    public DeathState deathState;

    public LayerMask detectionLayer;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyManager.isDead)
        {
            return deathState;
        }

        #region Handle Enemy Target Detection
        /*Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStats.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //Check for team ID

                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyStats.minimumDetectionAngle && viewableAngle < enemyStats.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }*/
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
