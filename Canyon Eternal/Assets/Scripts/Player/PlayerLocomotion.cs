﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerStats playerStats;
    PlayerManager playerManager;
    PlayerParticleHandler playerParticleHandler;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    public Transform dashFXTransform;
    private bool dashFXTriggered;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();
    }

    #region Movement

    public void HandleMovement()
    {
        playerManager.currentMoveDirection.x = Mathf.RoundToInt(inputManager.moveInput.x);
        playerManager.currentMoveDirection.y = Mathf.RoundToInt(inputManager.moveInput.y);

        playerManager.rb.MovePosition(playerManager.rb.position + playerManager.currentMoveDirection.normalized * playerStats.characterData.moveSpeed * Time.fixedDeltaTime);

        if (playerManager.currentMoveDirection.x == 0 && playerManager.currentMoveDirection.y == 0)
        {
            playerAnimatorHandler.UpdateIntAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
            playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
        }
        else
        {
            playerManager.lastMoveDirection = playerManager.currentMoveDirection;
            playerAnimatorHandler.UpdateIntAnimationValues(playerManager.currentMoveDirection.x, playerManager.currentMoveDirection.y, true);
            playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.currentMoveDirection.x, playerManager.currentMoveDirection.y, true);
        }
    }
    #endregion

    #region Dash
    public void HandleDash()
    {
            if(dashTime <= 0)
            {
                StopDash();
            }
            else
            {
                if(!dashFXTriggered)
                {
                    StartDash();
                }

                dashTime -= Time.deltaTime;
                playerManager.rb.AddForce(playerManager.lastMoveDirection * dashSpeed);
        }
    }

    private void StartDash()
    {
        dashFXTriggered = true;

        playerAnimatorHandler.PlayTargetAnimation("Dash", false);
        ReverseDashThroughEnemies();
        playerStats.LoseStamina(1);
        playerStats.EnableInvulnerability(startDashTime);

        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.dash);
        playerParticleHandler.SpawnBigDustCloudVFX();
    }

    private void StopDash()
    {
        dashFXTriggered = false;
        playerManager.isDashing = false;

        dashTime = startDashTime;
        ReverseDashThroughEnemies();
    }

    private void ReverseDashThroughEnemies()
    {
        Physics2D.IgnoreLayerCollision(9, 10, playerManager.isDashing);
        Physics2D.IgnoreLayerCollision(8, 11, playerManager.isDashing);
    }

    #endregion

    #region Handle Ground Layers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerManager.isDashing)
        {
            if (collision.gameObject.layer == 13)
            {
                dashTime = 0;
                SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.hardCollision, 0.4f);
                CinemachineShake.Instance.Shake(12, 0.35f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && !playerManager.isDashing)
        {
            if(!playerManager.isFalling)
            {
                StartCoroutine(HandleFalling());
            }
        }
    }

    private IEnumerator HandleFalling()
    {
        playerManager.isFalling = true;
        CinemachineShake.Instance.SwitchToFallCam();
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        playerAnimatorHandler.PlayTargetAnimation("Fall", true);
        playerAnimatorHandler.UpdateFloatAnimationValues(0, -1, false);

        yield return new WaitForSeconds(0.4f);

        InvokeRepeating("ApplyFallForce", 0.4f, 0.0001f);

        yield return new WaitForSeconds(0.45f);

        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.falling, 0.5f);

        CancelInvoke("ApplyFallForce");
        playerManager.isFalling = false;
        StartCoroutine(playerManager.HandleDeathCoroutine());
    }

    private void ApplyFallForce()
    {
        playerParticleHandler.SpawnBigDustCloudVFX();
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerManager.rb.AddForce(Vector2.down * 20000f);
    }

    #endregion
}
