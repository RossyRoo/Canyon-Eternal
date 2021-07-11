﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    float currentSpellChargeTime;
    GameObject projectilePointerVFXGO;
    [HideInInspector] public GameObject currentSpellGO;

    [Header("Parameters")]
    public GameObject projectilePointerVFXPrefab;

    [Header("Spell Casting SFX")]
    public AudioClip cancelSpellSFX;
    public AudioClip chargeSpellSFX;
    public AudioClip chargeSpellCompleteSFX;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();

        if (playerInventory.activeSpell == null)
        {
            if (playerInventory.spellsInventory.Count != 0)
            {
                playerInventory.activeSpell = playerInventory.spellsInventory[0];
            }
        }
    }


    public void ChargeSpell()
    {
        if (playerStats.currentStamina < playerInventory.activeSpell.staminaCost
            || playerInventory.activeSpell.isBuff && playerStats.isBuffed)
            return;

        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerMeleeHandler.currentMeleeModel.SetActive(false);

        playerAnimatorHandler.PlayTargetAnimation("Charge", true);
        playerParticleHandler.SpawnChargeVFX(playerInventory.activeSpell.chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellSFX, 0.05f);

        currentSpellChargeTime = playerInventory.activeSpell.chargeTime;
        playerManager.isChargingSpell = true;
    }

    public void CancelSpell()
    {
        playerAnimatorHandler.PlayTargetAnimation("Cancel", false);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(cancelSpellSFX, 0.1f);

        playerManager.isChargingSpell = false;
        playerMeleeHandler.currentMeleeModel.SetActive(true);
    }

    public void TickSpellChargeTimer()
    {
        if (playerManager.isChargingSpell)
        {
            currentSpellChargeTime -= Time.deltaTime;

            if (currentSpellChargeTime < 0)
            {
                currentSpellChargeTime = playerInventory.activeSpell.chargeTime;
                CompleteSpellCharge();
            }
        }
    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(playerInventory.activeSpell.staminaCost);

        playerAnimatorHandler.PlayTargetAnimation("ChargeComplete", true);
        playerParticleHandler.SpawnChargeCompleteVFX(playerInventory.activeSpell.chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellCompleteSFX);

        if (playerInventory.activeSpell.isProjectile)
        {
            currentSpellGO = Instantiate(playerInventory.activeSpell.GOPrefab, transform.position, Quaternion.identity);
            currentSpellGO.transform.parent = playerManager.transform;

            projectilePointerVFXGO = Instantiate(projectilePointerVFXPrefab, transform.position, Quaternion.identity);
            projectilePointerVFXGO.transform.parent = transform;
            StartCoroutine(RotatePointer());
        }
        else
        {
            HandleAllSpellCasting();
        }
    }

    private IEnumerator RotatePointer()
    {

        if (playerManager.isCastingSpell && playerInventory.activeSpell.isProjectile)
        {
            int lastMoveDirectionXInt = Mathf.RoundToInt(playerManager.lastMoveDirection.x);
            int lastMoveDirectionYInt = Mathf.RoundToInt(playerManager.lastMoveDirection.y);
            Vector2 lastMoveDirectionInt = new Vector2(lastMoveDirectionXInt, lastMoveDirectionYInt);

            if (lastMoveDirectionInt == Vector2.up)
            {
                projectilePointerVFXGO.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else if (lastMoveDirectionInt == Vector2.down)
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
            else if (lastMoveDirectionInt == new Vector2(-1, 1))
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
        else
        {
            yield break;
        }


        yield return new WaitForFixedUpdate();
        StartCoroutine(RotatePointer());
    }

    public void HandleAllSpellCasting()
    {
        if(playerManager.isCastingSpell)
        {
            if(playerInventory.activeSpell.isProjectile)
            {
                CastProjectile();
            }
            else if(playerInventory.activeSpell.isAOE)
            {
                CastAOE();
            }
            else
            {
                StartCoroutine(CastBuff());
            }

            playerManager.isCastingSpell = false;
            playerAnimatorHandler.PlayTargetAnimation("Cast", false);
            SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.activeSpell.launchSFX);
            playerMeleeHandler.currentMeleeModel.SetActive(true);
        }

    }

    private void CastProjectile()
    {
        Destroy(projectilePointerVFXGO);
        currentSpellGO.GetComponent<ProjectilePhysics>().Launch(playerInventory.activeSpell.launchForce, playerManager.lastMoveDirection);
    }

    private void CastAOE()
    {
        currentSpellGO = Instantiate(playerInventory.activeSpell.GOPrefab, transform.position, Quaternion.identity);
        currentSpellGO.transform.parent = playerManager.transform;

        //currentSpellGO.GetComponent<ProjectilePhysics>().Launch(activeSpell.launchForce, playerManager.lastMoveDirection);
        Destroy(currentSpellGO, 0.5f);
        Debug.Log("Casting AOE Spell");

    }

    private IEnumerator CastBuff()
    {
        playerStats.isBuffed = true;
        playerStats.ActivateHealthBuff(playerInventory.activeSpell.heartBuff);
        playerStats.ActivateStaminaBuff(playerInventory.activeSpell.staminaBuff);

        if (playerInventory.activeSpell.damagaMultiplierBuff != 0)
        {
            playerInventory.weaponsInventory[0].minDamage *= playerInventory.activeSpell.damagaMultiplierBuff;
            playerInventory.weaponsInventory[0].maxDamage *= playerInventory.activeSpell.damagaMultiplierBuff;
        }

        yield return new WaitForSeconds(playerInventory.activeSpell.buffDuration);

        playerStats.DeactivateHealthBuff();
        playerStats.DeactivateStaminaBuff();
        playerInventory.weaponsInventory[0].minDamage = playerInventory.weaponsInventory[0].startingMinDamage;
        playerInventory.weaponsInventory[0].maxDamage = playerInventory.weaponsInventory[0].startingMaxDamage;

        playerStats.isBuffed = false;
    }
}
