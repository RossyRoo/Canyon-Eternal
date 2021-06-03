﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : CharacterAnimatorEvents
{
    PlayerManager playerManager;
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerBlockHandler playerBlockHandler;
    PlayerStats playerStats;

    PlayerParticleHandler playerParticleHandler;

    ObjectPool objectPool;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        playerBlockHandler = GetComponentInParent<PlayerBlockHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();

        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Melee Events



    public void OpenDamageCollider()
    {
        playerMeleeHandler.AddComboDamage();

        playerStats.EnableInvulnerability(playerStats.hurtInvulnerabilityTime);

        playerMeleeHandler.meleeDamageCollider.EnableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = true;
    }

    public void CloseDamageCollider()
    {
        PlayMissedMeleeSFX();

        playerMeleeHandler.RevertComboDamage();

        playerMeleeHandler.meleeDamageCollider.DisableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = false;
    }

    #region Enable/Disable Combo Windows

    public void EnableComboWindow()
    {
        if(!playerMeleeHandler.comboWasMissed)
        {
            playerMeleeHandler.canContinueCombo = true;
            playerParticleHandler.SpawnComboStar();
        }
    }

    public void DisableComboWindow()
    {
        if(!playerMeleeHandler.comboWasHit)
        { 
            playerParticleHandler.ChangeComboStarColor(2);
        }
        playerMeleeHandler.canContinueCombo = false;
    }

    #endregion

    private void PlayMissedMeleeSFX()
    {
        DamageCollider myMeleeCollider = GetComponentInChildren<DamageCollider>();

        if (!myMeleeCollider.targetIsWithinRange)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.meleeWeaponSFXBank.attacks[playerMeleeHandler.comboNumber]);
        }
    }

    #endregion

    #region Blocking

    public void DespawnShield()
    {
        playerBlockHandler.shieldModel.SetActive(false);
    }

    #endregion

    public void Footstep()
    {
        int instantiatedFootstepsCount = 0;

        ParticleSystem []instantiatedFootsteps = objectPool.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < instantiatedFootsteps.Length; i++)
        {
            if(instantiatedFootsteps[i].gameObject.name == "footstep_vfx")
            {
                instantiatedFootstepsCount++;
            }
        }

        if(instantiatedFootstepsCount <= 2)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(characterSFXBank.rockFootsteps
                [Random.Range(0, characterSFXBank.rockFootsteps.Length)], 0.1f);

            playerParticleHandler.SpawnFootstepCloudVFX();
        }
        else
        {
            return;
        }

    }

}
