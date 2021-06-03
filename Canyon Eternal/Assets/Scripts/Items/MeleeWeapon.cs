using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cards/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    public bool isThrust;
    public bool isSlash;
    public bool isStrike;

    public GameObject modelPrefab;

    [Header("Attack Animations")]
    public string attackAnimation;

    public MeleeWeaponSFXBank meleeWeaponSFXBank;
}
