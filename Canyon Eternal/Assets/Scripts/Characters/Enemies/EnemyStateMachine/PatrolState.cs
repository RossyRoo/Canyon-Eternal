using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PatrolState : EnemyStateMachine
{
    [Header("STATE TRANSITIONS")]
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    [Header("Patrol Parameters")]
    public Transform startPosition;
    public Vector2 nextPosition;
    public float currentDistanceFromStartPosition;
    public float maxDistanceFromStartPosition;
    public float minIdleTime = 0f;
    public float maxIdleTime = 0f;

    [Header("Pathfinding")]
    public bool patrolPathfindingInitiated;
    public bool isIdlingAtNextPosition;
    Path path;
    public float nextWaypointDistance = 3f;
    public int currentWaypoint = 0;


    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {

        #region Handle Target Detection

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), enemyStats.characterData.detectionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerStats playerStats = colliders[i].GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                enemyManager.currentTarget = playerStats;
            }
        }
        #endregion

        #region Handle Switch State

        if (enemyManager.isDead)
        {
            return deathState;
        }

        if (enemyManager.isStunned)
        {
            return stunnedState;
        }

        if (enemyManager.currentTarget != null && enemyStats.characterData.canPursue)
        {
            enemyManager.EngagePlayer();
            enemyManager.isMoving = true;
            return pursueState;
        }

        #endregion

        float distanceFromNextPosition = Vector2.Distance(enemyManager.rb.position, nextPosition);

        if (distanceFromNextPosition < 1 || nextPosition == Vector2.zero)
        {
            FindNewPath();
        }


        if (!patrolPathfindingInitiated)
        {
            StartCoroutine(InitiatePatrolPathfinding(enemyManager));
            patrolPathfindingInitiated = true;
        }

        MoveTowardsPatrolPosition(enemyManager, enemyStats);


        return this;

    }


    private IEnumerator InitiatePatrolPathfinding(EnemyManager enemyManager)
    {
        if(isIdlingAtNextPosition)
        {
            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
            isIdlingAtNextPosition = false;
        }
        
        if (enemyManager.seeker.IsDone())
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, nextPosition, OnPathComplete);
        }
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(InitiatePatrolPathfinding(enemyManager));
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void MoveTowardsPatrolPosition(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        if (path == null)
            return;

        Vector2 patrolDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;

        enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 2) * Time.deltaTime); //I'm dividing movespeed here bc i dont think we need a variable for patrolling speed

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void FindNewPath()
    {
        isIdlingAtNextPosition = true;

        currentDistanceFromStartPosition = Vector2.Distance(transform.position, startPosition.position);

        if (currentDistanceFromStartPosition > maxDistanceFromStartPosition)
        {
            nextPosition = startPosition.position;
        }
        else
        {
            Vector2 xLimits = new Vector2((startPosition.position.x - maxDistanceFromStartPosition), (startPosition.position.x + maxDistanceFromStartPosition));
            Vector2 yLimits = new Vector2((startPosition.position.y - maxDistanceFromStartPosition), (startPosition.position.y + maxDistanceFromStartPosition));
            Vector2 tryPosition = new Vector2((Random.Range(xLimits.x, xLimits.y)), (Random.Range(yLimits.x, yLimits.y)));

            if(Vector2.Distance(transform.position, tryPosition) > 10f)
            {
                nextPosition = tryPosition;
            }
            else
            {
                FindNewPath();
            }
        }
    }

}