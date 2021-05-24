using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SFX Banks/Melee Weapon SFX")]
public class MeleeWeaponSFXBank : SFXBank
{
    [Header("SFX")]
    public AudioClip swingWeapon;
    public AudioClip collideWithEnemy;
}
