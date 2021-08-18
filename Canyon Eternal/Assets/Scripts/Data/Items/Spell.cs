using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Weapon/Spell")]
public class Spell : Weapon
{
    [Header("SPELL PARAMETERS")]
    public bool isProjectile;
    public bool isAOE;
    public bool isBuff;
    [Range(0.5f, 3f)]public float chargeTime;
    public GameObject chargeVFX;
    public GameObject chargeCompleteVFX;

    [Header("Projectile / AOE")]
    [Range(0, 10)] public float explosionRadius;
    public float launchForce;
    public GameObject GOPrefab;
    public AudioClip launchSFX;
    public AudioClip collisionSFX;

    [Header("Buffs")]
    public float buffDuration = 0f;
    public int heartBuff = 0;
    public int staminaBuff = 0;
    public float damagaMultiplierBuff = 0;
}
