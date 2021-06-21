using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The main change I want to make to this class is to allow it to additionally play the SFX in the function by accessing the character data in PlayerStats

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerSpellHandler playerSpellHandler;

    ObjectPool objectPool;

    public Transform mainTarget;
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
        playerSpellHandler = GetComponentInParent<PlayerSpellHandler>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Dust Clouds

    public void SpawnBigDustCloudVFX()
    {
        GameObject dashParticleVFXGO = Instantiate(bigDustVFX, new Vector2(mainTarget.position.x, mainTarget.position.y - 2), Quaternion.identity);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnLittleDustCloudVFX()
    {
        GameObject footstepVFXGO = Instantiate(littleDustVFX, new Vector2(mainTarget.position.x, mainTarget.position.y - 2), Quaternion.identity);
        footstepVFXGO.transform.parent = objectPool.transform;
        footstepVFXGO.name = "footstep_vfx";
        Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #endregion

    public void SpawnHealVFX()
    {
        PlayerStats playerStats = GetComponentInParent<PlayerStats>();

        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.consumeHealItem[playerStats.currentLunchBoxCapacity - 1]);

        GameObject healVFXGO = Instantiate(healVFX, mainTarget.position, Quaternion.identity);
        healVFXGO.GetComponent<ParticleSystemRenderer>().material = healMats[playerStats.currentLunchBoxCapacity - 1];
        healVFXGO.transform.parent = playerManager.transform;

        Destroy(healVFXGO, healVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnImpactVFX()
    {
        GameObject impactVFXGO = Instantiate(impactVFXPrefab, mainTarget.position, Quaternion.identity);
        impactVFXGO.transform.parent = objectPool.transform;
        Destroy(impactVFXGO, impactVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #region Spells
    public void SpawnChargeVFX(GameObject chargeVFXPrefab)
    {
        currentChargeVFXGO = Instantiate(chargeVFXPrefab, new Vector2(mainTarget.position.x, mainTarget.position.y - 2.7f), Quaternion.identity);
        currentChargeVFXGO.transform.parent = playerManager.transform;
    }

    public void SpawnChargeCompleteVFX(GameObject chargeCompleteVFXPrefab)
    {
        GameObject chargeCompleteVFXGO = Instantiate(chargeCompleteVFXPrefab, mainTarget.position, Quaternion.identity);
        chargeCompleteVFXGO.transform.parent = playerManager.transform;
        Destroy(chargeCompleteVFXGO, chargeCompleteVFXGO.GetComponent<ParticleSystem>().main.duration);
    }


    public void SpawnCastVFX(GameObject castVFXPrefab)
    {
        playerSpellHandler.currentSpellGO = Instantiate(castVFXPrefab, new Vector2(mainTarget.position.x, mainTarget.position.y - 1.5f), Quaternion.identity);
        playerSpellHandler.currentSpellGO.transform.parent = playerManager.transform;
    }
    #endregion


}
