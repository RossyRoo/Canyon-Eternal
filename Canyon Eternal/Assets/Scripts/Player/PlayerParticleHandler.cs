using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
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
    public GameObject currentComboStarGO;

    public Material yellowComboStarMat;
    public Material greenComboStarMat;
    public Material redComboStarMat;

    private void Awake()
    {
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
    }

    public void ChangeComboStarMat()
    {
        if(currentComboStarGO != null)
        {
            if (playerMeleeHandler.comboWasHit)
            {
                currentComboStarGO.GetComponent<ParticleSystemRenderer>().material = greenComboStarMat;
            }
        }
        else
        {
            if(playerMeleeHandler.comboWasMissed)
            {
                comboStarVFX.GetComponent<ParticleSystemRenderer>().material = redComboStarMat;
            }
        }
    }

    public void ResetComboStarMaterial()
    {
        comboStarVFX.GetComponent<ParticleSystemRenderer>().material = yellowComboStarMat;
    }
}
