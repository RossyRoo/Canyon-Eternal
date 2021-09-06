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

    public AudioClip[] footstepSFX;
    public AudioClip fallingSFX;
    public AudioClip hardCollisionSFX;

    [Header("Dash Settings")]
    public AudioClip dashSFX;
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
        if (!playerManager.isDead && !playerManager.isFalling && !playerManager.isInteractingWithUI)
        {
            playerManager.currentMoveDirection.x = Mathf.RoundToInt(inputManager.moveInput.x);
            playerManager.currentMoveDirection.y = Mathf.RoundToInt(inputManager.moveInput.y);

            if (!playerManager.isChargingSpell && !playerManager.isCastingSpell)
            {
                playerManager.rb.MovePosition(playerManager.rb.position +
                    playerManager.currentMoveDirection.normalized * playerStats.characterData.moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                playerManager.rb.MovePosition(playerManager.rb.position +
                    playerManager.currentMoveDirection.normalized * (playerStats.characterData.moveSpeed / 2) * Time.fixedDeltaTime);
            }

            //UPDATE ANIMS
            if (playerManager.currentMoveDirection.x == 0 && playerManager.currentMoveDirection.y == 0)
            {
                playerAnimatorHandler.UpdateIntAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
                playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, false);
                //playerAnimatorHandler.UpdateSprite(playerManager.lastMoveDirection.x, playerManager.lastMoveDirection.y, playerStats.characterData);
            }
            else
            {
                playerManager.lastMoveDirection = playerManager.currentMoveDirection;
                playerAnimatorHandler.UpdateIntAnimationValues(playerManager.currentMoveDirection.x, playerManager.currentMoveDirection.y, true);
                playerAnimatorHandler.UpdateFloatAnimationValues(playerManager.currentMoveDirection.x, playerManager.currentMoveDirection.y, true);
                //playerAnimatorHandler.UpdateSprite(playerManager.currentMoveDirection.x, playerManager.currentMoveDirection.y, playerStats.characterData);
            }
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

        SFXPlayer.Instance.PlaySFXAudioClip(dashSFX);
        playerParticleHandler.SpawnBigDustCloudVFX();
    }

    public void StopDash()
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            if (playerManager.isDashing)
            {
                dashTime = 0;
                SFXPlayer.Instance.PlaySFXAudioClip(hardCollisionSFX, 0.4f);
                CinemachineManager.Instance.Shake(12, 0.35f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && !playerManager.isDashing)
        {
            if(!playerManager.isFalling && !playerManager.isDead)
            {
                StartCoroutine(HandleFalling());
            }
        }
    }

    private IEnumerator HandleFalling()
    {
        Debug.Log("Run Falling Coroutine");
        playerManager.isFalling = true;

        playerAnimatorHandler.UpdateFloatAnimationValues(0, -1, false);

        yield return new WaitForSeconds(0.75f);

        CinemachineManager.Instance.LosePlayer();

        SFXPlayer.Instance.PlaySFXAudioClip(fallingSFX, 0.5f);
        playerParticleHandler.SpawnBigDustCloudVFX();

        StartCoroutine(playerManager.HandleDeathCoroutine("Fall"));
        CancelInvoke("ApplyFallForce");
        playerManager.isFalling = false;
    }

    #endregion
}
