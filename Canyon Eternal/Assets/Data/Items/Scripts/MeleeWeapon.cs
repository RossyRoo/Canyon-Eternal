using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Melee")]
public class MeleeWeapon : Weapon
{
    [Header("MELEE")]
    public bool isThrust;
    public bool isSlash;
    public bool isStrike;

    [Tooltip("The force used to calculate forward momentum during combo attacks")]
    public float attackMomentum = 1000;
    [Tooltip("Time between attack start and opening damage collider")]
    public float openDamageColliderBuffer = 0f;
    [Tooltip("Time between opening damage collider and closing damage collider")]
    public float closeDamageColliderBuffer = 0.15f;

    public GameObject modelPrefab;

    public string [] attackAnimations;

    [Header("SFX")]
    public AudioClip[] attackSFX;
    public AudioClip collideWithEnemySFX;
}
