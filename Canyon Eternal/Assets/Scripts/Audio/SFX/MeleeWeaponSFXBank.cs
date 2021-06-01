using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Banks/Melee Weapon SFX")]
public class MeleeWeaponSFXBank : AudioBank
{
    [Header("SFX")]
    public AudioClip [] attacks;
    public AudioClip collideWithEnemy;
}
