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
    public StunnedState stunnedState;

    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;
    bool pathfindingInitiated = false;
    [HideInInspector]

    public Vector2 moveForce;



    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {


        if (!pathfindingInitiated)
        {
            StartCoroutine(InitiatePathfinding(enemyManager));
            pathfindingInitiated = true;
        }

        MoveTowardsTarget(enemyManager);

        enemyManager.distanceFromTarget = Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position);

        #region Handle State Switching

        if (enemyManager.distanceFromTarget > enemyManager.blindDistance)
        {
            enemyManager.currentTarget = null;
            return scoutState;
        }

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            enemyManager.isMoving = false;
            return combatState;
        }
        else
        {
            return this;
        }
        #endregion

    }

    private IEnumerator InitiatePathfinding(EnemyManager enemyManager)
    {
        if (enemyManager.seeker.IsDone() && enemyManager.currentTarget != null)
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

        /*if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }*/

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