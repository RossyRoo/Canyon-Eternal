﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ReturnState : EnemyStateMachine
{
    public ScoutState scoutState;
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

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

        if (enemyManager.distanceFromTarget <= enemyStats.characterData.detectionRadius)//MIGHT ENGAGE
        {
            return pursueState;
        }

        if (Vector2.Distance(enemyManager.transform.position, scoutState.startPosition.position) <= 5f)
        {
            return scoutState;
        }

        #endregion

        #region Move to Start Position
        if (!pursuePathfindingInitiated)
        {
            Debug.Log("Going home initiated");
            pursuePathfindingInitiated = true;
            StartCoroutine(InitiateReturnPathfinding(enemyManager));
        }

        MoveTowardsTarget(enemyManager, enemyStats);

        Debug.Log("Distance from start point: " + Vector2.Distance(enemyManager.transform.position, scoutState.startPosition.position));
        #endregion

        return this;
    }

    private IEnumerator InitiateReturnPathfinding(EnemyManager enemyManager)
    {
        if (enemyManager.seeker.IsDone())
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, scoutState.startPosition.position, OnPathComplete);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(InitiateReturnPathfinding(enemyManager));
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
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

}