using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : Item
{
    [Header("Card Information")]

    public int staminaCost = 1;
    public int minDamage;
    public int maxDamage;
    [Range(0,1)]public float criticalChance;
    public int criticalDamage;

    public float cardKnockback;
}
