using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Item
{
    [Header("Weapon Information")]
    public int staminaCost = 1;

    [Header("Damage Stats")]
    public int baseMinDamage;
    public int baseMaxDamage;
    [HideInInspector] public int currentMinDamage;
    [HideInInspector] public int currentMaxDamage;
    [Range(0,1)]public float criticalChance;

    [Tooltip("Force used to knock enemy back upon attacks")]
    [HideInInspector]public float knockbackForce = 100f;

    public GameObject collisionVFX;

}
