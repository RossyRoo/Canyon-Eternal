using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyStats enemyStats;
    EnemyAnimatorHandler enemyAnimatorHandler;
    Animator animator;
    [HideInInspector] public Rigidbody2D rb;

    public EnemyStateMachine currentState;
    [HideInInspector] public CharacterStats currentTarget;

    [Header("Enemy Action Settings")]
    public float maximumAttackRange = 1.5f;
    public float currentRecoveryTime;


    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
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
