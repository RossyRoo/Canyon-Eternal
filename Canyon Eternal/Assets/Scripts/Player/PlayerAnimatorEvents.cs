using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerManager playerManager;

    PlayerParticleHandler playerParticleHandler;
    Animator animator;

    ObjectPool objectPool;



    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();
        animator = GetComponent<Animator>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Handle Melee Animation Events

    public void SpawnMelee()
    {
        playerMeleeHandler.currentMeleeModel.SetActive(true);
    }

    public void DespawnMelee()
    {
        if(playerMeleeHandler.comboNumber==0)
        {
            Debug.Log("Despawning Melee");
            playerMeleeHandler.currentMeleeModel.SetActive(false);
        }
    }

    public void OpenDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.EnableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = true;
    }

    public void CloseDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.DisableDamageCollider();
        playerMeleeHandler.attackMomentumActivated = false;
    }

    public void PlayWeaponSwingSFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.meleeWeaponSFXBank.swingWeapon);
    }

    public void EnableComboWindow()
    {
        playerMeleeHandler.canContinueCombo = true;

        if(playerMeleeHandler.comboNumber<2)
        {
            GameObject comboActivatedVFXGO =
                Instantiate(playerParticleHandler.comboActivatedVFX, playerParticleHandler.particleTransform.position, Quaternion.identity);
            comboActivatedVFXGO.transform.parent = objectPool.transform;
            Destroy(comboActivatedVFXGO, comboActivatedVFXGO.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public void DisableComboWindow()
    {
        playerMeleeHandler.canContinueCombo = false;

        if(playerMeleeHandler.comboNumber >= 2)
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }

        if (playerMeleeHandler.comboWasHit && !playerMeleeHandler.comboWasMissed)
        {
            playerMeleeHandler.comboNumber++;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }
        else
        {
            playerMeleeHandler.comboNumber = 0;
            animator.SetInteger("comboNumber", playerMeleeHandler.comboNumber);
        }

        playerMeleeHandler.comboWasHit = false;
        playerMeleeHandler.comboWasMissed = false;
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
