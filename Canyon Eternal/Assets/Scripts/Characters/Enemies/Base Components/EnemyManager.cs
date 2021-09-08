using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyManager : CharacterManager
{
    EnemyStats enemyStats;
    EnemyAnimatorHandler enemyAnimatorHandler;
    public GameObject myBodySprite;

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

        if (enemyStats.characterData.isSingleEncounter && FindObjectOfType<PlayerStats>().GetComponentInChildren<PlayerProgression>().enemiesEncountered.Contains(enemyStats.characterData))
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    private void Start()
    {
        GenerateTrackingWall();
        SetUpEnemyInventory();

        if(currentState == null)
        {
            currentState = GetComponentInChildren<ScoutState>();
        }
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        Rotate();

        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        UpdateMoveDirection();
    }

    #region State Machine

    private void SetUpEnemyInventory()
    {
        for (int i = 0; i < enemyStats.characterData.consumableItems.Count; i++)
        {
            GetComponentInChildren<ItemState>().myConsumables.Add(enemyStats.characterData.consumableItems[i]);
        }
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

        if (isInteracting)
        {
            if (currentRecoveryTime <= 0 && isStunned == false)
            {
                isInteracting = false;
            }
        }
    }

    #endregion

    private void UpdateMoveDirection()
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
            playerManager.enemiesEngaged.Remove(this);
        }
    }

    private void Rotate()
    {
        float offset = -90f;

        if (currentTarget != null)
        {
            Vector2 direction = (Vector2)currentTarget.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        else
        {
            float angle = Mathf.Atan2(currentMoveDirection.y, currentMoveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

    }


}
