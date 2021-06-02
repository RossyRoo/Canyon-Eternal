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
        if (playerStats.currentStamina < 1 || playerManager.isInteracting)
        {
            playerManager.isDashing = false;
            return;
        }

        ReverseDashThroughEnemies();

        if (direction == 0 && playerManager.moveDirection == Vector2.zero)
        {
            if(playerManager.lastMoveDirection == new Vector2(0,1))
            {
                direction = 1;
            }
            else if(playerManager.lastMoveDirection == new Vector2(0,-1))
            {
                direction = 2;
            }
            else if(playerManager.lastMoveDirection == new Vector2(-1,0))
            {
                direction = 3;
            }
            else if(playerManager.lastMoveDirection == new Vector2(1,0))
            {
                direction = 4;
            }
        }
        else if (direction == 0 && playerManager.moveDirection != Vector2.zero)
        {
            if (playerManager.moveDirection == new Vector2(0, 1))
            {
                direction = 1;
            }
            else if (playerManager.moveDirection == new Vector2(0,-1))
            {
                direction = 2;
            }
            else if (playerManager.moveDirection == new Vector2(-1,0))
            {
                direction = 3;
            }
            else if (playerManager.moveDirection == new Vector2(1,0))
            {
                direction = 4;
            }
            if (playerManager.moveDirection == new Vector2(1, -1))
            {
                direction = 5;
            }
            else if (playerManager.moveDirection == new Vector2(-1, -1))
            {
                direction = 6;
            }
            else if (playerManager.moveDirection == new Vector2(1, 1))
            {
                direction = 7;
            }
            else if (playerManager.moveDirection == new Vector2(-1, 1))
            {
                direction = 8;
            }
        }
        else
        {
            if(dashTime <= 0)
            {
                dashFXTriggered = false;
                direction = 0;
                dashTime = startDashTime;
                playerManager.rb.velocity = Vector2.zero;
                playerManager.isDashing = false;
                playerStats.currentStamina -= 1;
                ReverseDashThroughEnemies();
            }
            else
            {
                dashTime -= Time.deltaTime;

                if(direction == 1)
                {
                    playerManager.rb.AddForce(Vector2.up * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 2)
                {
                    playerManager.rb.AddForce(Vector2.down * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 3)
                {
                    playerManager.rb.AddForce(Vector2.left * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 4)
                {
                    playerManager.rb.AddForce(Vector2.right * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                if (direction == 5)
                {
                    playerManager.rb.AddForce(new Vector2(1,-1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 6)
                {
                    playerManager.rb.AddForce(new Vector2(-1, -1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 7)
                {
                    playerManager.rb.AddForce(new Vector2(1, 1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 8)
                {
                    playerManager.rb.AddForce(new Vector2(-1, 1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
            }
        }
    }

    private void PlayDashFX(Quaternion rotation)
    {
        if (dashFXTriggered)
            return;

        dashFXTriggered = true;

        playerStats.EnableInvulnerability(startDashTime);
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterSFXBank.dash);
        GameObject dashParticleVFXGO = Instantiate(playerParticleHandler.dashVFX, dashFXTransform.position, rotation);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    private void ReverseDashThroughEnemies()
    {
        Physics2D.IgnoreLayerCollision(9, 10, playerManager.isDashing);
        Physics2D.IgnoreLayerCollision(8, 11, playerManager.isDashing);
    }
    #endregion


}
