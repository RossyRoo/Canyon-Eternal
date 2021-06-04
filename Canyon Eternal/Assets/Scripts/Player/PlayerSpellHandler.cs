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

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
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

    public void CastProjectileSpell()
    {
        if(playerManager.isCastingSpell)
        {
            playerAnimatorHandler.PlayTargetAnimation(activeSpell.castAnimation, false);
            playerParticleHandler.SpawnCastVFX(activeSpell.castVFX);
            SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.castSFX);

            playerManager.isCastingSpell = false;
        }
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
                playerAnimatorHandler.PlayTargetAnimation(activeSpell.chargeCompleteAnimation, true);
                playerParticleHandler.SpawnChargeCompleteVFX(activeSpell.chargeCompleteVFX);
                Destroy(playerParticleHandler.currentChargeVFXGO);
                SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.chargeCompleteSFX);


                Debug.Log("CHARGE COMPLETE VFX");

                playerStats.LoseStamina(activeSpell.staminaCost);
                playerManager.isChargingSpell = false;
                playerManager.isCastingSpell = true;
                playerManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                currentSpellChargeTime = startSpellChargeTime;
            }
        }

    }
}
