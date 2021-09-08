using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ReturnState : EnemyStateMachine
{
    public ScoutState scoutState;
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region Move to Start Position


        Debug.Log("RETURNING TO: " + scoutState.startPosition.position);

        #endregion

        #region State Switching

        if (enemyManager.isDead)//MIGHT DIE
        {
            return deathState;
        }

        if (enemyManager.isStunned)//MIGHT BECOME STUNNED
        {
            return stunnedState;
        }

        if (enemyManager.distanceFromTarget <= enemyStats.characterData.detectionRadius)//MIGHT ENGAGE
        {
            return pursueState;
        }

        if (Vector2.Distance(enemyManager.transform.position, scoutState.startPosition.position) <= 5f)
        {
            Debug.Log("Made it home");
            return scoutState;
        }

        #endregion


        return this;
    }

}
