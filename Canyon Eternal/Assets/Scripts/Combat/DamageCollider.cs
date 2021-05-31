﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Tooltip("Assign if this damage collider belongs to a melee weapon")]
    public Card cardData;
    Collider2D damageCollider;

    int damage = 1;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackTime;
    private float startKnockbackTime = 0.1f;
    public Vector2 knockbackDirection;
    public bool knockbackFlag;
    public CharacterManager knockbackTarget;

    public bool dealsConstantDamage;
    public bool targetIsWithinRange;
    public bool canDamageEnemy = true;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        targetIsWithinRange = false;

        if (!dealsConstantDamage)
        {
            damageCollider.enabled = false;
        }

        if (cardData != null)
        {
            knockbackForce = cardData.cardKnockback;
        }

        knockbackTime = startKnockbackTime;
    }


    private void Update()
    {
        HandleKnockback();

    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager characterCollision = collision.gameObject.GetComponent<CharacterManager>();
        knockbackTarget = characterCollision;

        CharacterManager myCharacterManager = GetComponentInParent<CharacterManager>();
        knockbackDirection = myCharacterManager.facingDirection;


        if (characterCollision != null)
        {
            targetIsWithinRange = true;
            StartCoroutine(DealDamage(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targetIsWithinRange = false;
    }

    private IEnumerator DealDamage(GameObject collision)
    {
        if(!targetIsWithinRange)
        {
            yield break;
        }

        RollForCriticalHit();

        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.LoseHealth(damage);
            }
        }

        if (collision.tag == "Enemy" && canDamageEnemy)
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();

            if (enemyStats != null && !enemyManager.isDead)
            {
                enemyStats.LoseHealth(damage);
            }
        }

        knockbackTime = startKnockbackTime;
        knockbackFlag = true;

        yield return new WaitForFixedUpdate();
        StartCoroutine(DealDamage(collision));
    }


    private void HandleKnockback()
    {
        if (knockbackFlag)
        {
            if (knockbackTime <= 0)
            {
                knockbackFlag = false;
                knockbackTime = startKnockbackTime;
                knockbackTarget = null;
            }
            else
            {
                knockbackTime -= Time.deltaTime;

                knockbackTarget.rb.AddForce(knockbackDirection * knockbackForce);
            }
        }

    }

    private void RollForCriticalHit()
    {

        if (cardData != null)
        {
            float randValue = Random.value;

            if (randValue < 1 - cardData.criticalChance)
            {
                damage = Random.Range(cardData.minDamage, cardData.maxDamage);
            }
            else
            {
                damage = cardData.criticalDamage;
            }
        }
    }
}
