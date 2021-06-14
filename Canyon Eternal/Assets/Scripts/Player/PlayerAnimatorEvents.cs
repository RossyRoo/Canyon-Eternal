using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : CharacterAnimatorEvents
{
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
        playerParticleHandler = GetComponent<PlayerParticleHandler>();

        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Melee/Blocking

    public void OpenDamageCollider()
    {
        playerMeleeHandler.AddComboDamage();

        playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames);

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
            SFXPlayer.Instance.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.attackSFX[playerMeleeHandler.comboNumber]);
        }
    }

    public void PlayMissedBlockSFX()
    {
        BlockCollider blockCollider = GetComponentInChildren<BlockCollider>();

        if(!blockCollider.targetIsWithinRange)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.blockMissed);
        }
    }

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
            SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.rockFootsteps
                [Random.Range(0, playerStats.characterData.rockFootsteps.Length)], 0.1f);

            playerParticleHandler.SpawnLittleDustCloudVFX();
        }
        else
        {
            return;
        }

    }

}
