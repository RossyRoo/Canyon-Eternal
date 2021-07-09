using System.Collections;
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

    //public Spell activeSpell;

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
    }


    public void ChargeSpell()
    {
        if (playerStats.currentStamina < playerInventory.spellsInventory[0].staminaCost
            || playerInventory.spellsInventory[0].isBuff && playerStats.isBuffed)
            return;

        playerAnimatorHandler.animator.SetBool("isMoving", false);
        playerMeleeHandler.currentMeleeModel.SetActive(false);

        playerAnimatorHandler.PlayTargetAnimation("Charge", true);
        playerParticleHandler.SpawnChargeVFX(playerInventory.spellsInventory[0].chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellSFX, 0.05f);

        currentSpellChargeTime = playerInventory.spellsInventory[0].chargeTime;
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
                currentSpellChargeTime = playerInventory.spellsInventory[0].chargeTime;
                CompleteSpellCharge();
            }
        }
    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(playerInventory.spellsInventory[0].staminaCost);

        playerAnimatorHandler.PlayTargetAnimation("ChargeComplete", true);
        playerParticleHandler.SpawnChargeCompleteVFX(playerInventory.spellsInventory[0].chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellCompleteSFX);

        if (playerInventory.spellsInventory[0].isProjectile)
        {
            currentSpellGO = Instantiate(playerInventory.spellsInventory[0].GOPrefab, transform.position, Quaternion.identity);
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

        if (playerManager.isCastingSpell && playerInventory.spellsInventory[0].isProjectile)
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
            if(playerInventory.spellsInventory[0].isProjectile)
            {
                CastProjectile();
            }
            else if(playerInventory.spellsInventory[0].isAOE)
            {
                CastAOE();
            }
            else
            {
                StartCoroutine(CastBuff());
            }

            playerManager.isCastingSpell = false;
            playerAnimatorHandler.PlayTargetAnimation("Cast", false);
            SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.spellsInventory[0].launchSFX);
            playerMeleeHandler.currentMeleeModel.SetActive(true);
        }

    }

    private void CastProjectile()
    {
        Destroy(projectilePointerVFXGO);
        currentSpellGO.GetComponent<ProjectilePhysics>().Launch(playerInventory.spellsInventory[0].launchForce, playerManager.lastMoveDirection);
    }

    private void CastAOE()
    {
        currentSpellGO = Instantiate(playerInventory.spellsInventory[0].GOPrefab, transform.position, Quaternion.identity);
        currentSpellGO.transform.parent = playerManager.transform;

        //currentSpellGO.GetComponent<ProjectilePhysics>().Launch(activeSpell.launchForce, playerManager.lastMoveDirection);
        Destroy(currentSpellGO, 0.5f);
        Debug.Log("Casting AOE Spell");

    }

    private IEnumerator CastBuff()
    {
        playerStats.isBuffed = true;
        playerStats.ActivateHealthBuff(playerInventory.spellsInventory[0].heartBuff);
        playerStats.ActivateStaminaBuff(playerInventory.spellsInventory[0].staminaBuff);

        if (playerInventory.spellsInventory[0].damagaMultiplierBuff != 0)
        {
            playerInventory.weaponsInventory[0].minDamage *= playerInventory.spellsInventory[0].damagaMultiplierBuff;
            playerInventory.weaponsInventory[0].maxDamage *= playerInventory.spellsInventory[0].damagaMultiplierBuff;
        }

        yield return new WaitForSeconds(playerInventory.spellsInventory[0].buffDuration);

        playerStats.DeactivateHealthBuff();
        playerStats.DeactivateStaminaBuff();
        playerInventory.weaponsInventory[0].minDamage = playerInventory.weaponsInventory[0].startingMinDamage;
        playerInventory.weaponsInventory[0].maxDamage = playerInventory.weaponsInventory[0].startingMaxDamage;

        playerStats.isBuffed = false;
    }
}
