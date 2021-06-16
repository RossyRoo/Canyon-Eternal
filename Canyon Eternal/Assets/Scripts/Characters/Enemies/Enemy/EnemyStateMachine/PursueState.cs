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

    [Header("Pathfinding Data")]
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool pathfindingInitiated = false;
    public Vector2 moveForce;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if (!pathfindingInitiated)
        {
            StartCoroutine(InitiatePathfinding(enemyManager));
            pathfindingInitiated = true;
        }

        MoveTowardsTarget(enemyManager, enemyStats);

        enemyManager.distanceFromTarget = Vector2.Distance(enemyManager.rb.position, enemyManager.currentTarget.transform.position);

        #region Handle State Switching

        if (enemyManager.distanceFromTarget > enemyStats.characterData.detectionRadius * 4f)
        {
            enemyManager.DisengagePlayer();
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

        if (enemyManager.distanceFromTarget <= enemyStats.characterData.attackRange)
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

    private void MoveTowardsTarget(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        if (path == null)
            return;

        Vector2 targetDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;

        enemyManager.rb.AddForce(targetDirection * enemyStats.characterData.moveSpeed * Time.deltaTime);


        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

}