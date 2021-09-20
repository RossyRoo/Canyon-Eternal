using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item/Consumable")]
public class Consumable : Item
{
    public AudioClip consumeSFX;
    public int healAmount;
    public GameObject useVFX;
}
