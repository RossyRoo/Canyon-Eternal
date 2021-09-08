using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The main change I want to make to this class is to allow it to additionally play the SFX in the function by accessing the character data in PlayerStats

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;

    ObjectPool objectPool;

    public Transform meleeHand;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DUST CLOUDS")]
    public GameObject littleDustVFX;
    public GameObject bigDustVFX;

    [Header("IMPACT")]
    public GameObject impactVFXPrefab;

    [Header("SPELLS")]
    public GameObject currentChargeVFXGO;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Dust Clouds

    public void SpawnBigDustCloudVFX()
    {
        GameObject dashParticleVFXGO = Instantiate(bigDustVFX, transform.position, Quaternion.identity);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnLittleDustCloudVFX()
    {
        GameObject footstepVFXGO = Instantiate(littleDustVFX, transform.position, Quaternion.identity);
        footstepVFXGO.transform.parent = objectPool.transform;
        footstepVFXGO.name = "footstep_vfx";
        Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #endregion

    public void SpawnHealVFX()
    {
        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.consumeHealItemSFX);

        GameObject healVFXGO = Instantiate(healVFX, transform.position, Quaternion.identity);
        healVFXGO.GetComponent<ParticleSystemRenderer>().material = healMats[playerStats.currentLunchBoxCapacity - 1];
        healVFXGO.transform.parent = playerManager.transform;

        Destroy(healVFXGO, healVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnImpactVFX()
    {
        GameObject impactVFXGO = Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
        impactVFXGO.transform.parent = objectPool.transform;
        Destroy(impactVFXGO, impactVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #region Spells
    public void SpawnChargeVFX(GameObject chargeVFXPrefab)
    {
        currentChargeVFXGO = Instantiate(chargeVFXPrefab, transform.position, Quaternion.identity);
        currentChargeVFXGO.transform.parent = playerManager.transform;
    }

    public void SpawnChargeCompleteVFX(GameObject chargeCompleteVFXPrefab)
    {
        GameObject chargeCompleteVFXGO = Instantiate(chargeCompleteVFXPrefab, transform.position, Quaternion.identity);
        chargeCompleteVFXGO.transform.parent = playerManager.transform;
        Destroy(chargeCompleteVFXGO, chargeCompleteVFXGO.GetComponent<ParticleSystem>().main.duration);
    }


    #endregion


}
