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
    public float currentMomentumTime = 0;

    public LayerMask collisionLayers;


    private void Awake()
    {
        enemyStats = GetComponentInParent<EnemyStats>();

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

        if (currentTarget != null)
        {
            distanceFromTarget = Vector2.Distance(rb.position, currentTarget.transform.position);
        }

        isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
        isAttacking = enemyAnimatorHandler.animator.GetBool("isAttacking");
    }

    private void FixedUpdate()
    {
        CheckForMovement(); //THIS SHOULD BE DONE IN THE STATES?
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
        else
        {
            isAttacking = false;
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

    #region Movement / Rotation
    
    private void CheckForMovement()
    {
        if (Mathf.RoundToInt(rb.velocity.x) > 0 || Mathf.RoundToInt(rb.velocity.y) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void Rotate()
    {
        float offset = -90f;
        float rotationSpeed = 10f;

        if (currentTarget != null && currentMomentumTime <= 0) //PURSUIT ROTATION
        {
            Vector2 direction = (Vector2)currentTarget.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (angle + offset)), Time.deltaTime * rotationSpeed);
        }
        else if(currentTarget != null && currentMomentumTime > 0) //ATTACK ROTATION
        {
            float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (angle + offset)), Time.deltaTime * rotationSpeed);
        }
        else  //IDLE ROTATION
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (angle + offset)), Time.deltaTime * rotationSpeed);
        }
    }

    public IEnumerator ApplyAttackMomentum(EnemyAttackAction currentAttack, Vector2 targetDirection)
    {
        if(currentMomentumTime == 0)
        {
            currentMomentumTime = currentAttack.chargeDuration;
        }

        if (currentMomentumTime > 0)
        {
            Vector2 chargeForce = (targetDirection.normalized * currentAttack.chargeForce * Time.deltaTime);
            rb.AddForce(chargeForce);
        }
        else
        {
            currentMomentumTime = 0;
            yield break;
        }

        yield return new WaitForEndOfFrame();

        currentMomentumTime -= Time.deltaTime;
        StartCoroutine(ApplyAttackMomentum(currentAttack, targetDirection));
    }

    #endregion

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




}
