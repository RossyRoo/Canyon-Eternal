using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cards/Melee Cards")]
public class MeleeCard : Card
{

    public bool isThrust;
    public bool isSlash;
    public bool isStrike;

    public GameObject modelPrefab;

    [Header("Attack Animations")]
    public string attackAnimation;

    public MeleeWeaponSFXBank meleeWeaponSFXBank;
}
