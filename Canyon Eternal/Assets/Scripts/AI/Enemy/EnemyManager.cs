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
    public Rigidbody2D rb;
    [HideInInspector]
    public Seeker seeker;

    public EnemyStateMachine currentState;
    public PlayerStats currentTarget;

    [Header("Enemy Action Settings")]
    public float maximumAttackRange = 1.5f;
    public float currentRecoveryTime;


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
    }


    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
        animator.SetBool("isDead", isDead);
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



}
