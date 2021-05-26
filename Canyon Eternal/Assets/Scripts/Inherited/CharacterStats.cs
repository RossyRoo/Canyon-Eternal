using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    CharacterManager characterManager;

    public float moveSpeed = 12f;

    [Header("Data")]
    public CharacterSFXBank characterSFXBank;

    [Header("Health and Stamina")]
    public int maxHealth;
    public int currentHealth;
    public int healAmount;
    public float maxStamina;
    [HideInInspector]
    public float currentStamina;

    public float hurtInvulnerabilityTime = 0.8f;


    [Header("Loot Drops")]
    public int fragmentDrop; //THIS SHOULD GO IN CHARACTER INVENTORY

    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }


}
