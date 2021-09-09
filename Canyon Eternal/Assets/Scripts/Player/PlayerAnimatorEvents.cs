using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : CharacterAnimatorEvents
{
    PlayerLocomotion playerLocomotion;
    PlayerParticleHandler playerParticleHandler;

    ObjectPool objectPool;

    private void Awake()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerParticleHandler = GetComponent<PlayerParticleHandler>();

        objectPool = FindObjectOfType<ObjectPool>();
    }


    public void Footstep()
    {
        int instantiatedFootstepsCount = 0;

        ParticleSystem[] instantiatedFootsteps = objectPool.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < instantiatedFootsteps.Length; i++)
        {
            if (instantiatedFootsteps[i].gameObject.name == "footstep_vfx")
            {
                instantiatedFootstepsCount++;
            }
        }

        if (instantiatedFootstepsCount <= 3)
        {
            SFXPlayer.Instance.PlaySFXAudioClip(playerLocomotion.footstepSFX
                [Random.Range(0, playerLocomotion.footstepSFX.Length)], 0.1f);

            playerParticleHandler.SpawnLittleDustCloudVFX();
        }
        else
        {
            return;
        }

    }
}