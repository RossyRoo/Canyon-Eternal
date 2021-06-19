using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : CharacterAnimatorEvents
{
    PlayerStats playerStats;

    PlayerParticleHandler playerParticleHandler;

    ObjectPool objectPool;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
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

        if (instantiatedFootstepsCount <= 2)
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