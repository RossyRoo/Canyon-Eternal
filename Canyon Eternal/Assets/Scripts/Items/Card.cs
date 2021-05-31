using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : Item
{
    [Header("Card Information")]

    public int staminaCost = 1;

    [Header("Damage Stats")]
    public int baseMinDamage;
    public int baseMaxDamage;
    public int currentMinDamage;
    public int currentMaxDamage;
    [Range(0,1)]public float criticalChance;
    public int criticalDamage;
    public int comboDamageToAdd;

    public float cardKnockback;
    public float attackMomentum;
}
