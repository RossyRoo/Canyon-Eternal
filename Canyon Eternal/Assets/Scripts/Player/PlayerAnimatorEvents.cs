using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : CharacterAnimatorEvents
{
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerBlockHandler playerBlockHandler;
    PlayerStats playerStats;

    PlayerParticleHandler playerParticleHandler;

    ObjectPool objectPool;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        playerBlockHandler = GetComponentInParent<PlayerBlockHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();

        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Melee Events

    public void DespawnMelee()
    {
        if(!playerMeleeHandler.comboWasHit)
        {
            playerMeleeHandler.currentMeleeModel.SetActive(false);
            playerStats.isChainingInvulnerability = false;
        }
    }

    public void OpenDamageCollider()
    {
        playerMeleeHandler.AddComboDamage();

        playerStats.EnableInvulnerability(playerStats.hurtInvulnerabilityTime);
        playerStats.isChainingInvulnerability = true;

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
            playerParticleHandler.ChangeStarToRed();
        }
        playerMeleeHandler.canContinueCombo = false;
    }

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

            GameObject footstepVFXGO = Instantiate(playerParticleHandler.footstepVFX, playerParticleHandler.mainParticleTransform.position, Quaternion.identity);
            footstepVFXGO.transform.parent = objectPool.transform;
            footstepVFXGO.name = "footstep_vfx";
            Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
        }
        else
        {
            return;
        }

    }

}
