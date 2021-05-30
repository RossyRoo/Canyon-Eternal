using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : Item
{
    [Header("Card Information")]

    public int staminaCost = 1;
    public int damage;
    public float cardKnockback;
}
