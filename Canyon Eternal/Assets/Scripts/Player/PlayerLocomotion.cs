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
    public int direction;
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
        playerManager.moveDirection.x = Mathf.RoundToInt(inputManager.moveInput.x);
        playerManager.moveDirection.y = Mathf.RoundToInt(inputManager.moveInput.y);

        playerManager.rb.MovePosition(playerManager.rb.position + playerManager.moveDirection.normalized * playerStats.moveSpeed * Time.fixedDeltaTime);


        if (playerManager.moveDirection.x == 0 && playerManager.moveDirection.y == 0)
        {
            playerAnimatorHandler.UpdateIntAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
            playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
        }
        else
        {
            playerManager.lastMoveDirection = playerManager.moveDirection;
            playerAnimatorHandler.UpdateIntAnimationValues(playerManager.moveDirection.x, playerManager.moveDirection.y, true);
            playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.moveDirection.x, playerManager.moveDirection.y, true);
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

                direction = 0;
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
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterSFXBank.dash);
        playerParticleHandler.SpawnDashCloudVFX();
    }

    private void ReverseDashThroughEnemies()
    {
        Physics2D.IgnoreLayerCollision(9, 10, playerManager.isDashing);
        Physics2D.IgnoreLayerCollision(8, 11, playerManager.isDashing);
    }
    #endregion


}
