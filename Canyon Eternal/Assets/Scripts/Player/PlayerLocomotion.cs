using System.Collections;
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
        playerAnimatorHandler.PlayTargetAnimation("Dash", false);

        ReverseDashThroughEnemies();

            if(dashTime <= 0)
            {
                dashFXTriggered = false;

                dashTime = startDashTime;
                playerManager.rb.velocity = Vector2.zero;
                playerManager.isDashing = false;
                playerStats.LoseStamina(1);
                ReverseDashThroughEnemies();
            }
            else
            {
                dashTime -= Time.deltaTime;

                if(!dashFXTriggered)
                {
                    PlayDashFX();
                }

                playerManager.rb.AddForce(playerManager.lastMoveDirection * dashSpeed);
        }
    }

    private void PlayDashFX()
    {
        dashFXTriggered = true;
        playerStats.EnableInvulnerability(startDashTime);
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.dash);
        playerParticleHandler.SpawnDashCloudVFX();
    }

    private void ReverseDashThroughEnemies()
    {
        Physics2D.IgnoreLayerCollision(9, 10, playerManager.isDashing);
        Physics2D.IgnoreLayerCollision(8, 11, playerManager.isDashing);
    }
    #endregion


}
