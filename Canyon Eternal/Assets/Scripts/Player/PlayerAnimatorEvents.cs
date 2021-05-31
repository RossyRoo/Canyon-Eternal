using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerParticleHandler playerParticleHandler;
    Animator animator;


    private void Awake()
    {
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();
        animator = GetComponent<Animator>();
    }

    #region Handle Melee Animation Events

    public void DespawnMelee()
    {
        if(playerMeleeHandler.comboNumber==0)
        {
            playerMeleeHandler.currentMeleeModel.SetActive(false);
        }
    }

    public void OpenDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.DisableDamageCollider();
    }

    public void PlayWeaponSwingSFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.meleeWeaponSFXBank.swingWeapon);
    }

    public void EnableComboWindow()
    {
        playerMeleeHandler.canContinueCombo = true;
    }

    public void DisableComboWindow()
    {
        playerMeleeHandler.canContinueCombo = false;

        if(playerMeleeHandler.comboNumber >= 2)
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
            playerMeleeHandler.comboFlag = false;
        }

        if (playerMeleeHandler.comboFlag)
        {
            playerMeleeHandler.comboNumber++;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
            playerMeleeHandler.comboFlag = false;
        }
        else
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }
    }
    #endregion

    public void Footstep()
    {
        ObjectPool objectPool = FindObjectOfType<ObjectPool>();
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
                [Random.Range(0, characterSFXBank.rockFootsteps.Length)]);

            GameObject footstepVFXGO = Instantiate(playerParticleHandler.footstepVFX, playerParticleHandler.particleTransform.position, Quaternion.identity);
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
