using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Data / Character Data")]

public class CharacterData : ScriptableObject
{
    [Header("BEHAVIORS")]
    public bool canPursue;
    public bool canAttack;
    public bool canEvade;

    [Header("MOVEMENT")]
    [Range(3000, 8000)] public float moveSpeed = 5000f;

    [Header("HEALTH")]
    [Range(5, 2000)] public int maxHealth;
    public int currentHealth;
    public int healAmount;
    public float invulnerabilityFrames = 0.4f;

    [Header("Weapons And Attacks")]
    public EnemyAttackAction[] enemyAttacks; //Attacks enemy can use
    [HideInInspector] public DamageCollider[] enemyWeapons;   //Damage colliders on enemy

    [Header("DROPS")]
    public int fragmentDrop;

    [Header("SFX")]
    [Header("movement")]
    public AudioClip[] rockFootsteps;
    public AudioClip dash;

    [Header("combat")]
    public AudioClip[] consumeHealItem;
    public AudioClip block;
    public AudioClip takeNormalDamage;
    public AudioClip takeCriticalDamage;
    public AudioClip deathRattle;

    [Header("AI Detection")]
    [HideInInspector] public float detectionRadius = 35f;      //Distance at which enemy can spot the player
    [HideInInspector] public float attackRange = 0f;          //Distance enemy needs to enter attack state
    [HideInInspector] public float evadeRange = 5f;           //Distance enemy will back off target
}
