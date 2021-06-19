using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Data / Character Data")]

public class CharacterData : ScriptableObject
{
    [Header("ID")]
    public bool isBoss;
    public int enemyID;

    [Header("Sprites")]
    public Sprite[] torsoSprites;

    [Header("BEHAVIORS")]
    public bool canPursue;
    public bool canAttack;
    public bool canEvade;

    [Header("MOVEMENT")]
    [Range(0, 8000)] public float moveSpeed = 5000f;

    [Header("HEALTH")]
    [Range(4, 2000)] public int maxHealth;
    public int healAmount;
    public float invulnerabilityFrames = 0.4f;

    [Header("Weapons And Attacks")]
    public EnemyAttackAction[] enemyAttacks; //Attacks enemy can use

    [Header("DROPS")]
    public int fragmentDrop;

    [Header("SFX")]

    [Header("movement")]
    public AudioClip[] rockFootsteps;
    public AudioClip dash;
    public AudioClip hardCollision;
    public AudioClip falling;

    [Header("combat")]
    public AudioClip[] consumeHealItem;
    public AudioClip blockCollision;
    public AudioClip blockMissed;
    public AudioClip takeNormalDamage;
    public AudioClip takeCriticalDamage;
    public AudioClip deathRattle;

    public AudioClip cancelSpell;

    [Header("AI Detection")]
    public float detectionRadius = 15f;      //Distance at which enemy can spot the player
    [HideInInspector] public float attackRange = 0f;          //Distance enemy needs to enter attack state
    [HideInInspector] public float evadeRange = 5f;           //Distance enemy will back off target
}
