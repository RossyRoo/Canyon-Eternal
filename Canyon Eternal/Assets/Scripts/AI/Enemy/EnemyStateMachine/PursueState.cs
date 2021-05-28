using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PursueState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public ScoutState scoutState;
    public CombatState combatStanceState;
    public DeathState deathState;

    public float nextWaypointDistance = 3f;
    public float blindDistance = 25f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool pathfindingInitiated = false;


    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (enemyManager.isDead)
        {
            return deathState;
        }

        if(!pathfindingInitiated)
        {
            StartCoroutine(InitiatePathfinding(enemyManager));
            pathfindingInitiated = true;
        }

        HandleMoveTowardTarget(enemyManager);

        if (HandleLoseTarget(enemyManager))
        {
            return scoutState;
        }
        else
        {
            return this;
        }
    }

    private IEnumerator InitiatePathfinding(EnemyManager enemyManager)
    {
        if(enemyManager.seeker.IsDone() && enemyManager.currentTarget != null)
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, enemyManager.currentTarget.transform.position, OnPathComplete);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(InitiatePathfinding(enemyManager));
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void HandleMoveTowardTarget(EnemyManager enemyManager)
    {
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;
        Vector2 force = direction * enemyManager.enemyStats.moveSpeed * Time.deltaTime;

        enemyManager.rb.AddForce(force);

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }



    private bool HandleLoseTarget(EnemyManager enemyManager)
    {
        if (Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position) > blindDistance)
        {
            Debug.Log("Distance: " + Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position));
            enemyManager.currentTarget = null;
            Debug.Log("Target ran away");
            return true;
        }
        else return false;
    }
}