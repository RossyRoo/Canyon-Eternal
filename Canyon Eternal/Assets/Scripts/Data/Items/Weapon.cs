using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Item
{
    [Header("WEAPON PARAMETERS")]
    public int staminaCost = 1;
    public int startingMinDamage;
    public int startingMaxDamage;
    public float minDamage;
    public float maxDamage;
    [Tooltip("0 = Neutral. 1 = Earth. 2 = Flame. 3 = Air. 4 = Water. 5 = Poison. 6 = Psychic. 7 = Frost.")]
    public int damageType;
    [Range(0, 1)] public float statusChance = 0.5f;
    [Range(0,1)]public float criticalChance;
    public float knockbackForce = 100f;
    public GameObject [] collisionVFX;
    public AudioClip [] damageSFX;
    public AudioClip criticalDamageSFX;

}
