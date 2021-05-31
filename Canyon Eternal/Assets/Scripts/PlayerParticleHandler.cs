using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    public Transform particleTransform;

    [Header("HEALING")]
    public GameObject healVFX;
    public Material[] healMats;

    [Header("DASHING/WALKING")]
    public GameObject footstepVFX;
    public GameObject dashVFX;

    [Header("COMBAT")]
    public GameObject comboActivatedVFX;

}
