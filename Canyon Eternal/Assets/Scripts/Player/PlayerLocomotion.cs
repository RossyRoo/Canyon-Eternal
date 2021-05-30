using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerStats playerStats;
    PlayerManager playerManager;
    Rigidbody2D rb;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    public int direction;
    public Transform dashFXTransform;
    public GameObject dashParticleFXPrefab;

    private bool dashFXTriggered;

    public Vector2 moveDirection;
    public Vector2 lastMoveDirection;
    public bool isMoving;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();

        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleMovement()
    {
        moveDirection.x = Mathf.RoundToInt(inputManager.moveInput.x);
        moveDirection.y = Mathf.RoundToInt(inputManager.moveInput.y);

        lastMoveDirection.x = Mathf.RoundToInt(inputManager.lastMoveInput.x);
        lastMoveDirection.y = Mathf.RoundToInt(inputManager.lastMoveInput.y);

        rb.MovePosition(rb.position + moveDirection * playerStats.moveSpeed * Time.fixedDeltaTime);


        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            playerManager.facingDirection = lastMoveDirection;
            playerAnimatorHandler.UpdateMoveAnimationValues(lastMoveDirection.x, lastMoveDirection.y, false);
        }
        else
        {
            playerManager.facingDirection = moveDirection;
            playerAnimatorHandler.UpdateMoveAnimationValues(moveDirection.x, moveDirection.y, true);
        }
    }

    public void HandleDash()
    {
        if (playerStats.currentStamina < 1 || playerManager.isInteracting)
        {
            playerManager.dashFlag = false;
            return;
        }

        //Pass through enemies
        ReverseDashThroughEnemies();

        if (direction == 0 && moveDirection == Vector2.zero)
        {
            if(lastMoveDirection == new Vector2(0,1))
            {
                direction = 1;
            }
            else if(lastMoveDirection == new Vector2(0,-1))
            {
                direction = 2;
            }
            else if(lastMoveDirection == new Vector2(-1,0))
            {
                direction = 3;
            }
            else if(lastMoveDirection == new Vector2(1,0))
            {
                direction = 4;
            }
        }
        else if (direction == 0 && moveDirection != Vector2.zero)
        {
            if (moveDirection == new Vector2(0, 1))
            {
                direction = 1;
            }
            else if (moveDirection == new Vector2(0,-1))
            {
                direction = 2;
            }
            else if (moveDirection == new Vector2(-1,0))
            {
                direction = 3;
            }
            else if (moveDirection == new Vector2(1,0))
            {
                direction = 4;
            }
            if (moveDirection == new Vector2(1, -1))
            {
                direction = 5;
            }
            else if (moveDirection == new Vector2(-1, -1))
            {
                direction = 6;
            }
            else if (moveDirection == new Vector2(1, 1))
            {
                direction = 7;
            }
            else if (moveDirection == new Vector2(-1, 1))
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
                rb.velocity = Vector2.zero;
                playerManager.dashFlag = false;
                playerStats.currentStamina -= 1;
                ReverseDashThroughEnemies();
            }
            else
            {
                dashTime -= Time.deltaTime;

                if(direction == 1)
                {
                    rb.AddForce(Vector2.up * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 2)
                {
                    rb.AddForce(Vector2.down * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 3)
                {
                    rb.AddForce(Vector2.left * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if(direction == 4)
                {
                    rb.AddForce(Vector2.right * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                if (direction == 5)
                {
                    rb.AddForce(new Vector2(1,-1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 6)
                {
                    rb.AddForce(new Vector2(-1, -1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 7)
                {
                    rb.AddForce(new Vector2(1, 1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
                else if (direction == 8)
                {
                    rb.AddForce(new Vector2(-1, 1) * dashSpeed);
                    PlayDashFX(Quaternion.identity);
                }
            }
        }
    }

    private void PlayDashFX(Quaternion rotation)
    {
        if (dashFXTriggered)
            return;

        playerStats.EnableInvulnerability(startDashTime);
        playerAnimatorHandler.PlayTargetAnimation("Dash", false);
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterSFXBank.dash);
        GameObject dashParticleFXGO = Instantiate(dashParticleFXPrefab, dashFXTransform.position, rotation);
        dashParticleFXGO.transform.parent = null;
        Destroy(dashParticleFXGO, 1f);
        dashFXTriggered = true;
    }

    private void ReverseDashThroughEnemies()
    {
        Physics2D.IgnoreLayerCollision(9, 10, playerManager.dashFlag);
        Physics2D.IgnoreLayerCollision(8, 11, playerManager.dashFlag);
    }

}
