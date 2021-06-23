using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Spell")]
public class Spell : Projectile
{
    [Header("SPELL PARAMETERS")]
    public bool isProjectile;
    public bool isAOE;
    public bool isBuff;

    [Range(0.5f, 3f)]public float chargeTime;

    public GameObject chargeVFX;
    public GameObject chargeCompleteVFX;

    [Header("BUFF PARAMETERS")]
    public int heartBuff = 0;
    public int staminaBuff = 0;
    public int damagaMultiplierBuff = 0;
}
