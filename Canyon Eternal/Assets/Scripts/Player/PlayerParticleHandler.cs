using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The main change I want to make to this class is to allow it to additionally play the SFX in the function by accessing the character data in PlayerStats

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerSpellHandler playerSpellHandler;

    ObjectPool objectPool;

    public Transform overheadParticleTransform;
    public Transform torsoParticleTransform;
    public Transform feetParticleTransform;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DUST CLOUDS")]
    public GameObject footstepVFX;
    public GameObject dashVFX;
    public Transform dashFXTransform;

    [Header("IMPACT")]
    public GameObject impactVFXPrefab;

    [Header("COMBO STAR")]
    public GameObject comboStarVFX;
    public GameObject currentComboStarGO;
    public Material[] comboStarMats;

    [Header("SPELLS")]
    public GameObject currentChargeVFXGO;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerSpellHandler = GetComponentInParent<PlayerSpellHandler>();
        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Combo Star

    public void SpawnComboStar()
    {
        currentComboStarGO = Instantiate(comboStarVFX, new Vector2
            (overheadParticleTransform.position.x + (playerManager.currentMoveDirection.x * 2), overheadParticleTransform.position.y), Quaternion.identity);

        currentComboStarGO.transform.parent = gameObject.transform;
        Destroy(currentComboStarGO, currentComboStarGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void ChangeComboStarColor(int color)
    {
        if (currentComboStarGO != null)
        {
            currentComboStarGO.GetComponent<ParticleSystemRenderer>().material = comboStarMats[color];
        }
        else
        {
            comboStarVFX.GetComponent<ParticleSystemRenderer>().material = comboStarMats[color];
        }
    }

    #endregion

    #region Dust Clouds

    public void SpawnDashCloudVFX()
    {
        GameObject dashParticleVFXGO = Instantiate(dashVFX, dashFXTransform.position, Quaternion.identity);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnFootstepCloudVFX()
    {
        GameObject footstepVFXGO = Instantiate(footstepVFX, torsoParticleTransform.position, Quaternion.identity);
        footstepVFXGO.transform.parent = objectPool.transform;
        footstepVFXGO.name = "footstep_vfx";
        Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #endregion

    public void SpawnHealVFX()
    {
        PlayerStats playerStats = GetComponentInParent<PlayerStats>();

        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.consumeHealItem[playerStats.currentLunchBoxCapacity - 1]);

        GameObject healVFXGO = Instantiate(healVFX, torsoParticleTransform.position, Quaternion.identity);
        healVFXGO.GetComponent<ParticleSystemRenderer>().material = healMats[playerStats.currentLunchBoxCapacity - 1];
        healVFXGO.transform.parent = objectPool.transform;

        Destroy(healVFXGO, healVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnImpactVFX()
    {
        GameObject impactVFXGO = Instantiate(impactVFXPrefab, torsoParticleTransform.position, Quaternion.identity);
        impactVFXGO.transform.parent = objectPool.transform;
        Destroy(impactVFXGO, impactVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #region Spells
    public void SpawnChargeVFX(GameObject chargeVFXPrefab)
    {
        currentChargeVFXGO = Instantiate(chargeVFXPrefab, torsoParticleTransform.position, Quaternion.identity);
        currentChargeVFXGO.transform.parent = objectPool.transform;
    }

    public void SpawnChargeCompleteVFX(GameObject chargeCompleteVFXPrefab)
    {
        GameObject chargeCompleteVFXGO = Instantiate(chargeCompleteVFXPrefab, torsoParticleTransform.position, Quaternion.identity);
        chargeCompleteVFXGO.transform.parent = playerManager.transform;
        Destroy(chargeCompleteVFXGO, chargeCompleteVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnCancelSpellVFX(GameObject cancelSpellVFXPrefab)
    {
        GameObject cancelSpellVFXGO = Instantiate(cancelSpellVFXPrefab, torsoParticleTransform.position, Quaternion.identity);
        cancelSpellVFXGO.transform.parent = playerManager.transform;
        Destroy(cancelSpellVFXGO, cancelSpellVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnCastVFX(GameObject castVFXPrefab)
    {
        playerSpellHandler.currentSpellGO = Instantiate(castVFXPrefab, torsoParticleTransform.position, Quaternion.identity);
        playerSpellHandler.currentSpellGO.transform.parent = playerManager.transform;
    }
    #endregion

}
