using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMeleeHandler playerMeleeHandler;

    ObjectPool objectPool; 

    public Transform mainParticleTransform;
    public Transform critStarTransform;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DUST CLOUDS")]
    public GameObject footstepVFX;
    public GameObject dashVFX;
    public Transform dashFXTransform;

    [Header("COMBO STAR")]
    public GameObject comboStarVFX;
    public GameObject currentComboStarGO;
    public Material yellowComboStarMat;
    public Material greenComboStarMat;
    public Material redComboStarMat;

    public Material[] comboStarMats;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();

        objectPool = FindObjectOfType<ObjectPool>();
    }

    #region Combo Star

    public void SpawnComboStar()
    {
        currentComboStarGO = Instantiate(comboStarVFX, new Vector2
            (critStarTransform.position.x + (playerManager.moveDirection.x * 2), critStarTransform.position.y), Quaternion.identity);

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
        GameObject footstepVFXGO = Instantiate(footstepVFX, mainParticleTransform.position, Quaternion.identity);
        footstepVFXGO.transform.parent = objectPool.transform;
        footstepVFXGO.name = "footstep_vfx";
        Destroy(footstepVFXGO, footstepVFXGO.GetComponent<ParticleSystem>().main.duration);
    }

    #endregion

}
