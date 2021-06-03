using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyManager : CharacterManager
{
    [HideInInspector]
    public EnemyStats enemyStats;
    EnemyAnimatorHandler enemyAnimatorHandler;
    [HideInInspector]
    public Seeker seeker;

    public EnemyStateMachine currentState;
    public PlayerStats currentTarget;

    public DamageCollider[] myDamageColliders;

    [Header("Enemy Action Settings")]
    public EnemyAttackAction[] enemyAttacks;
    public float attackRange = 0f;
    [Tooltip("The distance at which the enemy will back off target")]
    public float evadeRange = 5f;
    public float blindDistance = 50f;
    public float currentRecoveryTime;

    [Header("Targeting")]
    public float distanceFromTarget;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    private void Start()
    {
        GenerateTrackingWall();

        myDamageColliders = GetComponentsInChildren<DamageCollider>();

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            if(enemyAttacks[i].shortestDistanceNeededToAttack > attackRange)
            {
                attackRange = enemyAttacks[i].shortestDistanceNeededToAttack;
            }
        }
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
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
            if (currentRecoveryTime <= 0)
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

}
