using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Consumable")]
public class Consumable : Item
{
    [Header("CONSUMABLES")]
    public AudioClip consumeSFX;
    public int healthAmount;
    public int staminaAmount;
    public bool isPermanentUpgrade;
    public GameObject useVFX;
}
