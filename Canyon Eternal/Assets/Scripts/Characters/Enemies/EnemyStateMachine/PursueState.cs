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

    [Header("Pathfinding Data")]
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool pursuePathfindingInitiated = false;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (!pursuePathfindingInitiated)
        {
            StartCoroutine(InitiatePursuePathfinding(enemyManager));
            pursuePathfindingInitiated = true;
        }

        MoveTowardsTarget(enemyManager, enemyStats);

        #region Handle State Switching

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

        if(enemyStats.characterData.summons.Count > 0)//Chance they will summon minions if they can
        {
            if (Random.value < 0.001f)
            {
                Debug.Log("Doing Summon");
                return summonState;
            }
        }

        return this;

        #endregion

    }

    private IEnumerator InitiatePursuePathfinding(EnemyManager enemyManager)
    {
        if (enemyManager.seeker.IsDone() && enemyManager.currentTarget != null)
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, enemyManager.currentTarget.transform.position, OnPathComplete);
        }
        yield return new WaitForSeconds(0.3f);
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
        if (path == null)
            return;

        Vector2 targetDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;
        enemyManager.distanceFromTarget = Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position);

        enemyManager.rb.AddForce(targetDirection * enemyStats.characterData.moveSpeed * Time.deltaTime);


        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


}