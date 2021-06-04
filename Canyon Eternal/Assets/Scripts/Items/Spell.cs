using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Spell")]
public class Spell : Weapon
{
    [Header("ANIMATIONS")]
    public string chargeAnimation;
    public string chargeCompleteAnimation;
    public string castAnimation;
    public string cancelAnimation;

    [Header("VFX")]
    public GameObject chargeVFX;
    public GameObject chargeCompleteVFX;
    public GameObject cancelVFX;
    public GameObject castVFX;
    public GameObject collisionVFX;

    [Header("SFX")]
    public AudioClip chargeSFX;
    public AudioClip chargeCompleteSFX;
    public AudioClip cancelSFX;
    public AudioClip castSFX;
    public AudioClip collisionSFX;

}
