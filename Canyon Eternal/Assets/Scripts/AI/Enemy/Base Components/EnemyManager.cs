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
    Animator animator;
    [HideInInspector]
    public Seeker seeker;

    public EnemyStateMachine currentState;
    public PlayerStats currentTarget;

    [Header("Enemy Action Settings")]
    public float maximumAttackRange = 1.5f;
    public float blindDistance = 50f;
    public float currentRecoveryTime;
    public float distanceFromTarget;

    public Vector2 movementDirection;
    public Vector2 lastFacingDirection = Vector2.down;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        rb.isKinematic = false;
        GenerateTrackingWall();
        InvokeRepeating("HandleRotation", 0f, 1f);
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
        animator.SetBool("isDead", isDead);

        GetFacingDirection();
        //UPDATE FACING DIRECTION
    }


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

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    private void HandleRotation()
    {
        Vector2 rawMoveDirection;

        rawMoveDirection.x = Mathf.RoundToInt(rb.velocity.x);
        rawMoveDirection.y = Mathf.RoundToInt(rb.velocity.y);

        if (rawMoveDirection.x == 0)
        {
            movementDirection.x = 0;
        }
        else if (rawMoveDirection.x > 0)
        {
            movementDirection.x = 1;
        }
        else
        {
            movementDirection.x = -1;
        }

        if (rawMoveDirection.y == 0)
        {
            movementDirection.y = 0;
        }
        else if (rawMoveDirection.y > 0)
        {
            movementDirection.y = 1;
        }
        else
        {
            movementDirection.y = -1;
        }

        if(movementDirection.x == 0 && movementDirection.y ==0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        enemyAnimatorHandler.UpdateMoveAnimationValues(movementDirection.x, movementDirection.y, isMoving);
    }

    private void GetFacingDirection()
    {
        if (movementDirection != Vector2.zero)
        {
            //limit to one direction
            facingDirection = movementDirection;

            lastFacingDirection = movementDirection;
        }
        else
        {
            facingDirection = lastFacingDirection;
        }

    }

}
