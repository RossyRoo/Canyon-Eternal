using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScoutState : EnemyStateMachine
{
    public Transform startPosition;

    [Header("STATE TRANSITIONS")]
    public PursueState pursueState;
    public DeathState deathState;
    public StunnedState stunnedState;

    [Header("BEHAVIOR")]
    public bool canPatrol;
    public bool canPathfind;
    public Transform[] waypoints;
    public bool randomizeWaypointOrder;

    [Header("Parameters")]
    public float maxDistanceFromStartPosition;
    public float minIdleTime = 0f;
    public float maxIdleTime = 0f;

    [Header("Debugging")]
    public Vector2 nextDestination;
    public int waypointIndex = 0;

    bool patrolPathfindingInitiated;
    bool isIdling = false;
    Path path;
    float nextWaypointDistance = 3f;
    int currentWaypoint = 0;


    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        #region Shed Start Position & Waypoints
        if (startPosition.parent == transform)
        {
            FindObjectOfType<ObjectPool>().AddToObjectPool(startPosition.gameObject, false);
            for (int i = 0; i < waypoints.Length; i++)
            {
                FindObjectOfType<ObjectPool>().AddToObjectPool(waypoints[i].gameObject, false);
            }
        }
        #endregion

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

        #region Handle Patrol

        if(canPatrol)
        {
            if (!patrolPathfindingInitiated)
            {
                if (canPathfind)
                {
                    StartCoroutine(FindDestination(enemyManager));
                    StartCoroutine(InitiatePatrolPathfinding(enemyManager));
                }
                else
                {
                    if (waypoints.Length != 0)
                    {
                        if (!randomizeWaypointOrder)
                        {
                            StartCoroutine(FollowPresetWaypoints(enemyManager, enemyStats));
                        }
                        else
                        {
                            waypointIndex = Random.Range(0, waypoints.Length);
                            StartCoroutine(FollowPresetWaypointsRandomly(enemyManager, enemyStats));
                        }
                    }
                }
                patrolPathfindingInitiated = true;
            }
            else
            {
                if (canPathfind)
                {
                    if (nextDestination == Vector2.zero)
                    {
                        nextDestination = transform.position;
                    }
                    if (Vector2.Distance(transform.position, nextDestination) < 7f)
                    {
                        StartCoroutine(FindDestination(enemyManager));
                    }

                    MoveTowardsPatrolPosition(enemyManager, enemyStats);

                }
            }
        }


        #endregion

        return this;

    }

    #region Waypoint Finding
    public IEnumerator FollowPresetWaypoints(EnemyManager enemyManager, EnemyStats enemyStats)
    {

        if (waypointIndex <= waypoints.Length - 1)
        {
            Vector2 patrolDirection = (waypoints[waypointIndex].position - transform.position).normalized;
            enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 3) * Time.deltaTime);


            if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) <= 1)
            {
                waypointIndex++;

                yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
            }
        }
        else
        {
            System.Array.Reverse(waypoints);
            waypointIndex = 1;
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine(FollowPresetWaypoints(enemyManager, enemyStats));
    }

    public IEnumerator FollowPresetWaypointsRandomly(EnemyManager enemyManager, EnemyStats enemyStats)
    {
        Vector2 patrolDirection = (waypoints[waypointIndex].position - transform.position).normalized;

        enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 3) * Time.deltaTime);


        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) <= 1)
        {
            waypointIndex = Random.Range(0, waypoints.Length);

            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        }

        yield return new WaitForFixedUpdate();
        StartCoroutine(FollowPresetWaypointsRandomly(enemyManager, enemyStats));
    }
    #endregion

    #region Pathfinding (Experimental)

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
        if (isIdling)
            return;

        float distance = Vector2.Distance(enemyManager.rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        Vector2 patrolDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyManager.rb.position).normalized;

        enemyManager.rb.AddForce(patrolDirection * (enemyStats.characterData.moveSpeed / 3) * Time.deltaTime); //I'm dividing movespeed here bc i dont think we need a variable for patrolling speed
    }

    public IEnumerator FindDestination(EnemyManager enemyManager)
    {
        {
            bool isLookingForDestination = true;

            while (isLookingForDestination)
            {
                Vector2 xLimits = new Vector2((startPosition.position.x - maxDistanceFromStartPosition), (startPosition.position.x + maxDistanceFromStartPosition));
                Vector2 yLimits = new Vector2((startPosition.position.y - maxDistanceFromStartPosition), (startPosition.position.y + maxDistanceFromStartPosition));
                Vector3 tryPosition = new Vector2((Random.Range(xLimits.x, xLimits.y)), (Random.Range(yLimits.x, yLimits.y)));

                Vector2 tryDirection = (tryPosition - transform.position);

                RaycastHit2D circleCastHit = Physics2D.CircleCast(tryPosition, 3f, Vector3.zero, enemyManager.collisionLayers); //Check if destination is in an obstacle
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, tryDirection, enemyManager.collisionLayers); //Check if there is an obstacle between enemy and destination

                if (circleCastHit.collider.gameObject.layer != enemyManager.collisionLayers
                    && raycastHit.collider.gameObject.layer != enemyManager.collisionLayers)
                {
                    isLookingForDestination = false;
                    nextDestination = tryPosition;
                }
            }

        }

        isIdling = true;
        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        isIdling = false;
    }
    #endregion

}