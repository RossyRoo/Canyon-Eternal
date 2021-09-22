using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerParticleHandler playerParticleHandler;

    float currentSpellChargeTime;
    [HideInInspector] public GameObject currentSpellGO;

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
        playerParticleHandler = GetComponentInChildren<PlayerParticleHandler>();

        if (playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] == null)
        {
            if (playerInventory.spellsInventory.Count != 0)
            {
                playerInventory.spellSlots[playerInventory.activeSpellSlotNumber] = playerInventory.spellsInventory[0];
            }
        }
    }

    public void TickSpellChargeTimer()
    {
        if (playerManager.isChargingSpell)
        {
            currentSpellChargeTime -= Time.deltaTime;

            if (currentSpellChargeTime < 0)
            {
                currentSpellChargeTime = playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].chargeTime;
                CompleteSpellCharge();
            }
        }
    }

    #region Charging
    public void ChargeSpell()
    {
        if (playerStats.currentStamina < playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].staminaCost
            || playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].isBuff && playerStats.isBuffed)
            return;

        playerMeleeHandler.currentMeleeModel.SetActive(false);

        playerParticleHandler.SpawnChargeVFX(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].chargeVFX);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellSFX, 0.05f);

        currentSpellChargeTime = playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].chargeTime;
        playerManager.isChargingSpell = true;
    }

    private void CompleteSpellCharge()
    {
        playerManager.isChargingSpell = false;
        playerManager.isCastingSpell = true;

        playerStats.LoseStamina(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].staminaCost);

        playerParticleHandler.SpawnChargeCompleteVFX(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].chargeCompleteVFX);
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(chargeSpellCompleteSFX);

        if (playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].isProjectile)
        {
            currentSpellGO = Instantiate(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].GOPrefab, transform.position, Quaternion.Euler(0, 0, 90));
            currentSpellGO.transform.parent = playerManager.transform;

        }
        else
        {
            HandleAllSpellCasting();
        }
    }
    #endregion

    #region Casting
    public void HandleAllSpellCasting()
    {
        if(playerManager.isCastingSpell)
        {
            if(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].isProjectile)
            {
                //CAST A PROJECTILE SPELL
                currentSpellGO.GetComponent<ProjectilePhysics>().Launch(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].launchForce, transform.up);
            }
            else if(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].isAOE)
            {
                //CAST AN AOE SPELL
                currentSpellGO = Instantiate(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].GOPrefab, transform.position, Quaternion.identity);
                currentSpellGO.transform.parent = playerManager.transform;

                Destroy(currentSpellGO, 0.5f);
            }
            else
            {
                //CAST A BUFF SPELL
                StartCoroutine(CastBuff());
            }

            playerManager.isCastingSpell = false;
            SFXPlayer.Instance.PlaySFXAudioClip(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].launchSFX);
            playerMeleeHandler.currentMeleeModel.SetActive(true);
        }

    }


    private IEnumerator CastBuff()
    {
        playerStats.isBuffed = true;
        playerStats.ActivateHealthBuff(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].heartBuff);
        playerStats.ActivateStaminaBuff(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].staminaBuff);

        if (playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].damagaMultiplierBuff != 0)
        {
            playerInventory.weaponsInventory[0].minDamage *= playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].damagaMultiplierBuff;
            playerInventory.weaponsInventory[0].maxDamage *= playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].damagaMultiplierBuff;
        }

        yield return new WaitForSeconds(playerInventory.spellSlots[playerInventory.activeSpellSlotNumber].buffDuration);

        playerStats.DeactivateHealthBuff();
        playerStats.DeactivateStaminaBuff();
        playerInventory.weaponsInventory[0].minDamage = playerInventory.weaponsInventory[0].startingMinDamage;
        playerInventory.weaponsInventory[0].maxDamage = playerInventory.weaponsInventory[0].startingMaxDamage;

        playerStats.isBuffed = false;
    }

    public void CancelSpell()
    {
        Destroy(playerParticleHandler.currentChargeVFXGO);
        SFXPlayer.Instance.PlaySFXAudioClip(cancelSpellSFX, 0.1f);

        playerManager.isChargingSpell = false;
        playerMeleeHandler.currentMeleeModel.SetActive(true);
    }
    #endregion

    #region Buffs



    #endregion
}
