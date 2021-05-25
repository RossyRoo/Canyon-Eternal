using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float moveSpeed = 12f;

    [Header("Data")]
    public CharacterSFXBank characterSFXBank;

    [Header("Health and Stamina")]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    public float maxStamina;
    [HideInInspector]
    public float currentStamina;

    public float hurtInvulnerabilityTime = 1f;


    [Header("Loot Drops")]
    public int fragmentDrop; //THIS SHOULD GO IN CHARACTER INVENTORY

}
