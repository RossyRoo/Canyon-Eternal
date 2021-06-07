using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The main change I want to make to this class is to allow it to additionally play the SFX in the function by accessing the character data in PlayerStats

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerSpellHandler playerSpellHandler;

    ObjectPool objectPool;

    public Transform cameraTarget;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DUST CLOUDS")]
    public GameObject littleDustVFX;
    public GameObject bigDustVFX;

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
            (cameraTarget.position.x + (playerManager.currentMoveDirection.x * 2), (cameraTarget.position.y + 3)), Quaternion.identity);

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

    public void SpawnBigDustCloudVFX()
    {
        GameObject dashParticleVFXGO = Instantiate(bigDustVFX, new Vector2(cameraTarget.position.x, cameraTarget.position.y - 2), Quaternion.identity);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnLittleDustCloudVFX()
    {
        GameObject footstepVFXGO = Instantiate(littleDustVFX, new Vector2(cameraTarget.position.x, cameraTarget.position.y - 2), Quaternion.identity);
        footstepVFXGO.transform.parent = objectPool.transform;
        footstepVFXGO.name = "footstep_vfx";
        Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #endregion

    public void SpawnHealVFX()
    {
        PlayerStats playerStats = GetComponentInParent<PlayerStats>();

        SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.consumeHealItem[playerStats.currentLunchBoxCapacity - 1]);

        GameObject healVFXGO = Instantiate(healVFX, cameraTarget.position, Quaternion.identity);
        healVFXGO.GetComponent<ParticleSystemRenderer>().material = healMats[playerStats.currentLunchBoxCapacity - 1];
        healVFXGO.transform.parent = playerManager.transform;

        Destroy(healVFXGO, healVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnImpactVFX()
    {
        GameObject impactVFXGO = Instantiate(impactVFXPrefab, cameraTarget.position, Quaternion.identity);
        impactVFXGO.transform.parent = objectPool.transform;
        Destroy(impactVFXGO, impactVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #region Spells
    public void SpawnChargeVFX(GameObject chargeVFXPrefab)
    {
        currentChargeVFXGO = Instantiate(chargeVFXPrefab, cameraTarget.position, Quaternion.identity);
        currentChargeVFXGO.transform.parent = playerManager.transform;
    }

    public void SpawnChargeCompleteVFX(GameObject chargeCompleteVFXPrefab)
    {
        GameObject chargeCompleteVFXGO = Instantiate(chargeCompleteVFXPrefab, cameraTarget.position, Quaternion.identity);
        chargeCompleteVFXGO.transform.parent = playerManager.transform;
        Destroy(chargeCompleteVFXGO, chargeCompleteVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnCancelSpellVFX(GameObject cancelSpellVFXPrefab)
    {
        GameObject cancelSpellVFXGO = Instantiate(cancelSpellVFXPrefab, cameraTarget.position, Quaternion.identity);
        cancelSpellVFXGO.transform.parent = playerManager.transform;
        Destroy(cancelSpellVFXGO, cancelSpellVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void SpawnCastVFX(GameObject castVFXPrefab)
    {
        playerSpellHandler.currentSpellGO = Instantiate(castVFXPrefab, cameraTarget.position, Quaternion.identity);
        playerSpellHandler.currentSpellGO.transform.parent = playerManager.transform;
    }
    #endregion

}
