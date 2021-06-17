using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyManager : CharacterManager
{
    EnemyStats enemyStats;
    EnemyAnimatorHandler enemyAnimatorHandler;

    [HideInInspector]
    public Seeker seeker;

    [Header("State Settings")]
    public EnemyStateMachine currentState;
    public PlayerStats currentTarget;
    public float distanceFromTarget;
    public float currentRecoveryTime;

    public LayerMask collisionLayers;



    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();

        if (FindObjectOfType<PlayerStats>().bossesDefeated.Contains(enemyStats.characterData.enemyID))
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        GetComponentInChildren<PatrolState>().startPosition.parent = FindObjectOfType<ObjectPool>().transform;
    }

    private void Start()
    {
        GenerateTrackingWall();

        if(currentState == null)
        {
            //currentState = GetComponentInChildren<ScoutState>();
            currentState = GetComponentInChildren<PatrolState>();
        }

    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        enemyAnimatorHandler.animator.SetBool("isInteracting", isInteracting);

        AvoidObstaclesWhilePatrolling();
    }

    private void FixedUpdate()
    {
        UpdateRotationByVelocity();
    }

    #region State Machine

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            EnemyStateMachine nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(EnemyStateMachine state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isInteracting)
        {
            if (currentRecoveryTime <= 0 && isStunned == false)
            {
                isInteracting = false;
            }
        }
    }

    #endregion

    private void UpdateRotationByVelocity()
    {
        Vector2 rawMoveDirection;

        rawMoveDirection.x = Mathf.RoundToInt(rb.velocity.x);
        rawMoveDirection.y = Mathf.RoundToInt(rb.velocity.y);

        if (rawMoveDirection.x == 0)
        {
            currentMoveDirection.x = 0;
        }
        else if (rawMoveDirection.x > 0)
        {
            currentMoveDirection.x = 1;
        }
        else
        {
            currentMoveDirection.x = -1;
        }

        if (rawMoveDirection.y == 0)
        {
            currentMoveDirection.y = 0;
        }
        else if (rawMoveDirection.y > 0)
        {
            currentMoveDirection.y = 1;
        }
        else
        {
            currentMoveDirection.y = -1;
        }

        if (currentMoveDirection != Vector2.zero)
        {
            lastMoveDirection = currentMoveDirection;
            enemyAnimatorHandler.UpdateFloatAnimationValues(currentMoveDirection.x, currentMoveDirection.y, isMoving);
            isMoving = true;
        }
        else
        {
            enemyAnimatorHandler.UpdateFloatAnimationValues(lastMoveDirection.x, lastMoveDirection.y, isMoving);
            isMoving = false;
        }
    }

    public void EngagePlayer()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();

        if (!playerManager.enemiesEngaged.Contains(this))
        {
            playerManager.enemiesEngaged.Add(this);
        }
    }

    public void DisengagePlayer()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();

        if (playerManager.enemiesEngaged.Contains(this))
        {
            Debug.Log("Lost the player");
            playerManager.enemiesEngaged.Remove(this);
        }
    }

    public void AvoidObstaclesWhilePatrolling()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 10f, lastMoveDirection, collisionLayers);

        if (hit)
        {
            PatrolState patrolState = GetComponentInChildren<PatrolState>();

            if (currentState == patrolState)
            {
                patrolState.FindNewPath();
            }
        }
    }

}
