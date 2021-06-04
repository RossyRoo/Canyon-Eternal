using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Melee")]
public class MeleeWeapon : Weapon
{
    public bool isThrust;
    public bool isSlash;
    public bool isStrike;

    public int comboDamageToAdd;

    public GameObject modelPrefab;

    public string attackAnimation;

    public MeleeWeaponSFXBank meleeWeaponSFXBank;
}
