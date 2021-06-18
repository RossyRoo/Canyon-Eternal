using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PatrolState : EnemyStateMachine
{
    public Transform startPosition;

    [Header("STATE TRANSITIONS")]
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    [Header("Behavior")]
    public bool canPatrol;
    public Transform[] presetWaypoints;

    [Header("Parameters")]
    public float maxDistanceFromStartPosition;
    public float minIdleTime = 0f;
    public float maxIdleTime = 0f;

    Vector2 nextDestination;
    float currentDistanceFromStartPosition;
    bool patrolPathfindingInitiated;
    bool isIdling = false;
    Path path;
    float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    int presetWaypointIndex = 0;


    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        if(startPosition.parent == transform)
        {
            startPosition.parent = null;
            for (int i = 0; i < presetWaypoints.Length; i++)
            {
                presetWaypoints[i].parent = null;
            }
        }

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
            StartCoroutine(enemyStats.characterBarkUI.DisplayBarkIcon(0));
            enemyManager.EngagePlayer();
            enemyManager.isMoving = true;
            return pursueState;
        }

        #endregion

        if(canPatrol)
        {
            if (!patrolPathfindingInitiated)
            {
                if (presetWaypoints.Length == 0)
                {
                    StartCoroutine(FindDestination());
                    StartCoroutine(InitiatePatrolPathfinding(enemyManager));
                }
                else
                {
                    StartCoroutine(FollowPresetWaypoints(enemyManager, enemyStats));
                }
                patrolPathfindingInitiated = true;
            }
            else
            {
                if (presetWaypoints.Length == 0)
                {
                    if (Vector2.Distance(enemyManager.rb.position, nextDestination) < 5)
                    {
                        StartCoroutine(FindDestination());
                    }

                    MoveTowardsPatrolPosition(enemyManager, enemyStats);
                }
            }


        }



        return this;

    }


    private IEnumerator InitiatePatrolPathfinding(EnemyManager enemyManager)
    {
        if (enemyManager.seeker.IsDone())
        {
            enemyManager.seeker.StartPath(enemyManager.rb.position, nextDestination, OnPathComplete);
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
        if (path == null || isIdling)
            return;

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        Vector2 patrolDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;

        enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 2) * Time.deltaTime); //I'm dividing movespeed here bc i dont think we need a variable for patrolling speed
    }

    public IEnumerator FindDestination()
    {
        currentDistanceFromStartPosition = Vector2.Distance(transform.position, startPosition.position);

        if (currentDistanceFromStartPosition > maxDistanceFromStartPosition)
        {
            nextDestination = startPosition.position;
        }
        else
        {
            Vector2 xLimits = new Vector2((startPosition.position.x - maxDistanceFromStartPosition), (startPosition.position.x + maxDistanceFromStartPosition));
            Vector2 yLimits = new Vector2((startPosition.position.y - maxDistanceFromStartPosition), (startPosition.position.y + maxDistanceFromStartPosition));

            nextDestination = new Vector2((Random.Range(xLimits.x, xLimits.y)), (Random.Range(yLimits.x, yLimits.y))); ;
        }
        isIdling = true;
        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        isIdling = false;
    }

    public IEnumerator FollowPresetWaypoints(EnemyManager enemyManager, EnemyStats enemyStats)
    {

        if (presetWaypointIndex <= presetWaypoints.Length - 1)
        {
            Vector2 patrolDirection = (presetWaypoints[presetWaypointIndex].position - transform.position).normalized;
            enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 2) * Time.deltaTime);


            if (Vector2.Distance(transform.position, presetWaypoints[presetWaypointIndex].position) <= 1)
            {
                presetWaypointIndex++;

                yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
            }
        }
        else
        {
            System.Array.Reverse(presetWaypoints);
            presetWaypointIndex = 1;
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine(FollowPresetWaypoints(enemyManager, enemyStats));
    }


}