using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public Card cardData;
    Collider2D damageCollider;

    int damage = 1;
    bool criticalHitActivated;

    [Header("Knockback Settings")]
    public CharacterManager knockbackTarget;
    public Vector2 knockbackDirection;
    public float knockbackForce = 10f;
    private float knockbackTime;
    [Tooltip("The length of time a knockback occurs")]
    public float startKnockbackTime = 0.02f;
    private bool knockbackFlag;

    [Tooltip("Check this for traps and projectiles")]
    public bool dealsConstantDamage;
    public bool targetIsWithinRange;
    [Tooltip("Uncheck if this is this is an object that can damage enemies")]
    public bool canDamageEnemy = true;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
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

    #region Enable/Disable Colliders

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
        Debug.Log("Damage collider ENABLED");
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        Debug.Log("Damage collider DISABLED");
    }
    #endregion

    #region OnTrigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager characterCollision = collision.gameObject.GetComponent<CharacterManager>();

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

    #endregion

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
                enemyStats.LoseHealth(damage, criticalHitActivated);
            }
        }

        if (cardData == null)
        {
            CharacterManager myCharacterManager = GetComponentInParent<CharacterManager>();
            knockbackDirection = myCharacterManager.moveDirection;
        }

        knockbackTarget = collision.GetComponent<CharacterManager>();
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
                knockbackTarget = null;
                knockbackTime = startKnockbackTime;
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
                damage = Random.Range(cardData.currentMinDamage, cardData.currentMaxDamage);
                criticalHitActivated = false;
            }
            else
            {
                damage = cardData.currentMaxDamage * 2;
                criticalHitActivated = true;
            }
        }
    }
}
