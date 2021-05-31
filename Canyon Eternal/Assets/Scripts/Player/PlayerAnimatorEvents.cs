using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    CharacterSFXBank characterSFXBank;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerParticleHandler playerParticleHandler;


    private void Awake()
    {
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
        characterSFXBank = GetComponentInParent<PlayerStats>().characterSFXBank;
        playerParticleHandler = GetComponent<PlayerParticleHandler>();
    }

    public void DespawnMelee()
    {
        playerMeleeHandler.currentMeleeModel.SetActive(false);
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
            Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().duration);
        }
        else
        {
            return;
        }

    }
}
