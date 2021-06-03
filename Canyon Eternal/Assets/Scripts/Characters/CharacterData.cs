using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Data / Character Data")]

public class CharacterData : ScriptableObject
{
    [Header("MOVEMENT")]
    [Range(3000, 8000)] public float moveSpeed = 5000f;

    [Header("HEALTH")]
    [Range(5, 2000)] public int maxHealth;
    public int currentHealth;
    public int healAmount;
    public float invulnerabilityFrames = 0.4f;

    [Header("DROPS")]
    public int fragmentDrop;

    [Header("SFX")]
    public CharacterSFXBank characterSFXBank;

    [Header("AI Detection")]
    [HideInInspector] public float detectionRadius = 35f;      //Distance at which enemy can spot the player
    [HideInInspector] public float attackRange = 0f;          //Distance enemy needs to enter attack state
    [HideInInspector] public float evadeRange = 5f;           //Distance enemy will back off target
}
