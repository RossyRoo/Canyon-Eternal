using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerParticleHandler playerParticleHandler;

    public Spell activeSpell;

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
        playerStats = GetComponent<PlayerStats>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();
    }


    public void ChargeSpell()
    {
        if (playerStats.currentStamina < activeSpell.staminaCost)
            return;

        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerMeleeHandler.currentMeleeModel.SetActive(false);

        playerAnimatorHandler.PlayTargetAnimation("Charge", true);
        playerParticleHandler.SpawnChargeVFX(activeSpell.chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellSFX, 0.05f);

        currentSpellChargeTime = activeSpell.chargeTime;
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
                currentSpellChargeTime = activeSpell.chargeTime;
                CompleteSpellCharge();
            }
        }
    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(activeSpell.staminaCost);

        playerAnimatorHandler.PlayTargetAnimation("ChargeComplete", true);
        playerParticleHandler.SpawnChargeCompleteVFX(activeSpell.chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellCompleteSFX);

        if (activeSpell.isProjectile)
        {
            playerParticleHandler.SpawnCastVFX(activeSpell.GOPrefab);

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

        if (playerManager.isCastingSpell && activeSpell.isProjectile)
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
            if(activeSpell.isProjectile)
            {
                CastProjectile();
            }
            else if(activeSpell.isAOE)
            {
                CastAOE();
            }
            else
            {
                CastBuff();
            }

            playerManager.isCastingSpell = false;
            playerAnimatorHandler.PlayTargetAnimation("Cast", false);
            SFXPlayer.Instance.PlaySFXAudioClip(activeSpell.launchSFX);
            playerMeleeHandler.currentMeleeModel.SetActive(true);
        }

    }

    private void CastProjectile()
    {
        Destroy(projectilePointerVFXGO);
        currentSpellGO.GetComponent<ProjectilePhysics>().Launch(activeSpell.launchForce, playerManager.lastMoveDirection);
    }

    private void CastAOE()
    {
        playerParticleHandler.SpawnCastVFX(activeSpell.GOPrefab);

        //currentSpellGO.GetComponent<ProjectilePhysics>().Launch(activeSpell.launchForce, playerManager.lastMoveDirection);
        Destroy(currentSpellGO, 0.5f);
        Debug.Log("Casting AOE Spell");

    }

    private void CastBuff()
    {
        playerStats.characterData.maxHealth += activeSpell.heartBuff;
        playerStats.RecoverHealth(activeSpell.heartBuff, false);

        playerStats.maxStamina += activeSpell.staminaBuff;
        //Reset max hearts and stamina so that only max is raised

        if (activeSpell.damagaMultiplierBuff != 0)
        {
            playerMeleeHandler.activeMeleeCard.minDamage *= activeSpell.damagaMultiplierBuff;
            playerMeleeHandler.activeMeleeCard.maxDamage *= activeSpell.damagaMultiplierBuff;
        }
        
    }
}
