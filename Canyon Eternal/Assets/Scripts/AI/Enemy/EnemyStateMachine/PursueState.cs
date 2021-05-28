using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PursueState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public ScoutState scoutState;
    public CombatState combatState;
    public DeathState deathState;

    public float nextWaypointDistance = 3f;
    public float blindDistance = 25f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool pathfindingInitiated = false;
    [HideInInspector]
    public Vector2 moveForce;


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

        MoveTowardsTarget(enemyManager);

        enemyManager.distanceFromTarget = Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position);

        if (enemyManager.distanceFromTarget > blindDistance)
        {
            enemyManager.currentTarget = null;
            return scoutState;
        }
        else if(enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatState;
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
        yield return new WaitForSeconds(0.3f);
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

    private void MoveTowardsTarget(EnemyManager enemyManager)
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
        moveForce = direction * enemyManager.enemyStats.moveSpeed * Time.deltaTime;

        enemyManager.rb.AddForce(moveForce);

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

}