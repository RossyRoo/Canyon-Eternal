using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Weapon/Offhand")]
public class OffhandWeapon : MeleeWeapon
{
    [Header("OFFHAND PARAMETERS")]
    public float blockSpeedMultiplier;
}
