﻿using System;
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

    public DamageCollider[] myDamageColliders;
    public EnemyAttackAction[] enemyAttacks;


    [Header("Enemy Action Settings")]
    public float maximumAttackRange = 0f;
    public float blindDistance = 50f;
    public float currentRecoveryTime;
    public float distanceFromTarget;

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

        myDamageColliders = GetComponentsInChildren<DamageCollider>();

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            if(enemyAttacks[i].shortestDistanceNeededToAttack > maximumAttackRange)
            {
                maximumAttackRange = enemyAttacks[i].shortestDistanceNeededToAttack;
            }
        }

    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
        animator.SetBool("isDead", isDead);

        GetFacingDirection();
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
            moveDirection.x = 0;
        }
        else if (rawMoveDirection.x > 0)
        {
            moveDirection.x = 1;
        }
        else
        {
            moveDirection.x = -1;
        }

        if (rawMoveDirection.y == 0)
        {
            moveDirection.y = 0;
        }
        else if (rawMoveDirection.y > 0)
        {
            moveDirection.y = 1;
        }
        else
        {
            moveDirection.y = -1;
        }

        if(moveDirection.x == 0 && moveDirection.y ==0)
        {
            //isMoving = false;
        }
        else
        {
            //isMoving = true;
        }

        enemyAnimatorHandler.UpdateFloatAnimationValues(moveDirection.x, moveDirection.y, isMoving);
    }

    private void GetFacingDirection()
    {
        if (moveDirection != Vector2.zero)
        {
            base.moveDirection = moveDirection;

            lastMoveDirection = moveDirection;
        }
        else
        {
            base.moveDirection = lastMoveDirection;
        }

    }

}
