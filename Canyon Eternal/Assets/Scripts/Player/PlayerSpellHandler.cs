using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    public Spell activeSpell;

    [Header("Charging")]
    public float currentSpellChargeTime;
    public float startSpellChargeTime = 2f;

    [Header("Projectiles")]
    public GameObject projectilePointerVFXGO;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
    }

    private void Update()
    {
        int lastMoveDirectionXInt = Mathf.RoundToInt(playerManager.lastMoveDirection.x);
        int lastMoveDirectionYInt = Mathf.RoundToInt(playerManager.lastMoveDirection.y);
        Vector2 lastMoveDirectionInt = new Vector2(lastMoveDirectionXInt, lastMoveDirectionYInt);
        Debug.Log(lastMoveDirectionInt);

        if (lastMoveDirectionInt == Vector2.up)
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if(lastMoveDirectionInt == Vector2.down)
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        else if (lastMoveDirectionInt == Vector2.left)
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else if (lastMoveDirectionInt == Vector2.right)
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(lastMoveDirectionInt == new Vector2(-1,1))
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 135);
        }
        else if (lastMoveDirectionInt == new Vector2(1, 1))
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 45);
        }
        else if (lastMoveDirectionInt == new Vector2(-1, -1))
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 225);
        }
        else if (lastMoveDirectionInt == new Vector2(1, -1))
        {
            projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 315);
        }

    }

    public void ChargeSpell()
    {
        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeAnimation, true);
        playerParticleHandler.SpawnChargeVFX(activeSpell.chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeSFX);


        currentSpellChargeTime = startSpellChargeTime;
        playerManager.isChargingSpell = true;
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void HandleAllSpellCasting()
    {
        if(playerManager.isCastingSpell)
        {
            if(activeSpell.isProjectile)
            {
                CastProjectile();
            }

            playerManager.isCastingSpell = false;
        }
    }

    private void CastProjectile()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.castAnimation, false);
        playerParticleHandler.SpawnCastVFX(activeSpell.castVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.castSFX);

        projectilePointerVFXGO.SetActive(false);
    }

    public void CancelSpell()
    {
        playerAnimatorHandler.PlayTargetAnimation(activeSpell.cancelAnimation, false);
        playerParticleHandler.SpawnCancelSpellVFX(activeSpell.cancelVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.cancelSFX);


        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerManager.isChargingSpell = false;
    }

    public void TickSpellChargeTimer()
    {
        if(playerManager.isChargingSpell)
        {
            currentSpellChargeTime -= Time.deltaTime;

            if (currentSpellChargeTime < 0)
            {
                currentSpellChargeTime = startSpellChargeTime;
                CompleteSpellCharge();
            }
        }

    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(activeSpell.staminaCost);
        playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeCompleteAnimation, true);
        playerParticleHandler.SpawnChargeCompleteVFX(activeSpell.chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeCompleteSFX);

        if(activeSpell.isProjectile)
        {
            projectilePointerVFXGO.SetActive(true);
        }
    }
}
