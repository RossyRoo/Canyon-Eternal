using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Spell")]
public class Spell : Weapon
{
    [Header("SPELL TYPE")]
    public bool isProjectile;

    [Header("SPELL SPEED")]
    [Range(0.5f, 3f)]public float chargeTime;
    [Range(10,150)]public float castSpeed;

    public GameObject spellDamageColliderPrefab;

    [Header("SPELL ANIMATIONS")]
    public string chargeAnimation;
    public string chargeCompleteAnimation;
    public string castAnimation;
    public string cancelAnimation;

    [Header("SPELL VFX")]
    public GameObject chargeVFX;
    public GameObject chargeCompleteVFX;
    public GameObject cancelVFX;
    public GameObject castVFX;
    public GameObject collisionVFX;

    [Header("SPELL SFX")]
    public AudioClip chargeSFX;
    public AudioClip chargeCompleteSFX;
    public AudioClip cancelSFX;
    public AudioClip castSFX;
    public AudioClip collisionSFX;

}
