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

    public int comboDamageToAdd;
    [Tooltip("The force used to calculate forward momentum during combo attacks")]
    [HideInInspector]public float attackMomentum = 1000;

    public GameObject modelPrefab;

    public string attackAnimation;

    [Header("SFX")]
    public AudioClip[] attackSFX;
    public AudioClip collideWithEnemySFX;
}
