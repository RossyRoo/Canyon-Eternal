using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMeleeHandler playerMeleeHandler;

    public Transform mainParticleTransform;
    public Transform critStarTransform;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DASHING/WALKING")]
    public GameObject footstepVFX;
    public GameObject dashVFX;

    [Header("COMBO STAR")]
    public GameObject comboStarVFX;
    GameObject currentComboStarGO;

    public Material yellowComboStarMat;
    public Material greenComboStarMat;
    public Material redComboStarMat;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
    }

    public void SpawnComboStar()
    {
        currentComboStarGO = Instantiate(comboStarVFX, new Vector2
            (critStarTransform.position.x + (playerManager.moveDirection.x * 2), critStarTransform.position.y), Quaternion.identity);

        currentComboStarGO.transform.parent = gameObject.transform;
        Destroy(currentComboStarGO, currentComboStarGO.GetComponent<ParticleSystem>().main.duration);
    }

    public void ChangeStarToYellow()
    {
        comboStarVFX.GetComponent<ParticleSystemRenderer>().material = yellowComboStarMat;
    }

    public void ChangeStarToGreen()
    {
        currentComboStarGO.GetComponent<ParticleSystemRenderer>().material = greenComboStarMat;
    }

    public void ChangeStarToRed()
    {
        if (currentComboStarGO != null)
        {
            currentComboStarGO.GetComponent<ParticleSystemRenderer>().material = redComboStarMat;
        }
        else
        {
            comboStarVFX.GetComponent<ParticleSystemRenderer>().material = redComboStarMat;
        }
    }
}
