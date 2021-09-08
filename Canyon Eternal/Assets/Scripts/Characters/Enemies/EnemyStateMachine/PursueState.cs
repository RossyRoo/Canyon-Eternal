using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PursueState : EnemyStateMachine
{

    [Header("STATE TRANSITIONS")]
    public ScoutState scoutState;
    public AttackState attackState;
    public EvadeState evadeState;
    public DeathState deathState;
    public StunnedState stunnedState;
    public ItemState itemState;
    public SummonState summonState;

    float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool pursuePathfindingInitiated = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region State Switching

        if (enemyManager.isDead)//MIGHT DIE
        {
            return deathState;
        }

        if (enemyManager.isStunned)//MIGHT BECOME STUNNED
        {
            return stunnedState;
        }

        if (enemyManager.distanceFromTarget > enemyStats.characterData.detectionRadius * 4f)//MIGHT DISENGAGE
        {
            enemyManager.DisengagePlayer();
            enemyManager.currentTarget = null;
            return scoutState;
        }

        if (enemyManager.distanceFromTarget < enemyStats.characterData.evadeRange && enemyStats.characterData.canEvade
            && !enemyManager.currentTarget.GetComponent<PlayerManager>().isDashing)//MIGHT EVADE
        {
            if (Random.value < 0.001f) //percent chance they will evade
            {
                return evadeState;
            }
        }

        if (enemyManager.currentRecoveryTime <= 0
            && enemyManager.distanceFromTarget <= enemyStats.characterData.attackRange
            && enemyStats.characterData.canAttack
            && !enemyManager.isInteracting)//Will attack if they can
        {
            return attackState;
        }

        if (enemyStats.characterData.consumableItems.Count > 0
            && !itemState.allConsumablesUsed
            && enemyStats.currentHealth <= (enemyStats.characterData.startingMaxHealth / 3))//Chance they will use item if they can
        {
            if (Random.value < 0.001f)
            {
                return itemState;
            }
        }

        if (enemyStats.characterData.summons.Count > 0)//Chance they will summon minions if they can
        {
            if (Random.value < 0.001f)
            {
                Debug.Log("Doing Summon");
                return summonState;
            }
        }

        #endregion

        
        #region Pursuit

        if (!pursuePathfindingInitiated)
        {
            pursuePathfindingInitiated = true;
            StartCoroutine(InitiatePursuePathfinding(enemyManager));
        }
        
        MoveTowardsTarget(enemyManager, enemyStats);
        
        #endregion

        return this;
    }

    #region Pathfinding

    private IEnumerator InitiatePursuePathfinding(EnemyManager enemyManager)
    {
        if (enemyManager.seeker.IsDone() && enemyManager.currentTarget != null)
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, enemyManager.currentTarget.transform.position, OnPathComplete);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(InitiatePursuePathfinding(enemyManager));
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void MoveTowardsTarget(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;
        Vector2 force = dir * enemyStats.characterData.moveSpeed * Time.deltaTime;

        enemyManager.rb.AddForce(force);

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    #endregion

}