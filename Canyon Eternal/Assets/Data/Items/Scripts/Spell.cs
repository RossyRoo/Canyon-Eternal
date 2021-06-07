using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Spell")]
public class Spell : Projectile
{
    [Header("SPELL")]
    public bool isProjectile;

    [Range(0.5f, 3f)]public float chargeTime;

    [Header("Animations")]
    public string chargeAnimation;
    public string chargeCompleteAnimation;
    public string castAnimation;
    public string cancelAnimation;

    [Header("vfx")]
    public GameObject chargeVFX;
    public GameObject chargeCompleteVFX;
    public GameObject cancelVFX;

    [Header("sfx")]
    public AudioClip chargeSFX;
    public AudioClip chargeCompleteSFX;
    public AudioClip cancelSFX;

}
