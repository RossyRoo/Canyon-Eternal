using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Item
{
    [Header("WEAPON PARAMETERS")]
    public int staminaCost = 1;
    public int minDamage;
    public int maxDamage;
    [Range(0,1)]public float criticalChance;
    public float knockbackForce = 100f;
    public GameObject collisionVFX;
    public AudioClip [] damageSFX;
    public AudioClip criticalDamageSFX;
}
