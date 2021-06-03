using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float moveSpeed = 12f;

    [Header("Data")]
    public CharacterSFXBank characterSFXBank;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    public int healAmount;
    public float hurtInvulnerabilityTime = 0.8f;

    [Header("Loot Drops")]
    public int fragmentDrop; //THIS SHOULD GO IN CHARACTER INVENTORY

}
