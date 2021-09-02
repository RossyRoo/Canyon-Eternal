using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character / Character")]

public class CharacterData : DataObject
{
    [Header("Basic Info")]
    public Contact characterContact;
    public int characterID;

    [Header("MOVEMENT")]
    [Range(0, 8000)] public float moveSpeed = 5000f;

    [Header("HEALTH")]
    [Range(4, 2000)] public int startingMaxHealth;
    public int currentMaxHealth;
    public int healAmount;
    public float invulnerabilityFrames = 0.4f;

    [Header("Sprites")]
    public Sprite[] torsoSprites;

    [Header("ENEMY STATS")]
    public bool isSingleEncounter;

    [Header("Behaviors")]
    public bool canPursue;
    public bool canAttack;
    public bool canEvade;

    [Header("AI Detection")]
    public float detectionRadius = 15f;      //Distance at which enemy can spot the player
    [Tooltip("Distance enemy needs to enter attack state")]
    public float attackRange = 0f;
    [Tooltip("Distance enemy will back off target")]
    public float evadeRange = 5f;

    [Header("Weapons And Attacks")]
    public EnemyAttackAction[] enemyAttacks; //Attacks enemy can use

    [Header("DROPS")]
    public List <Consumable> randomLootDrops;
    public List<Consumable> consumableItems;
    public int fragmentDrop;

    [Header("SFX")]
    
    public AudioClip deathRattleSFX;

    [Header("Summons")]
    public List<GameObject> summons;


}
