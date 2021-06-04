using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider2D damageCollider;

    [Header("Collider Type")]
    public bool dealsConstantDamage;
    public bool targetIsWithinRange;
    public bool canDamageEnemy = true;

    [Header("Knockback Settings")]
    public CharacterManager knockbackTarget;
    public Vector2 knockbackDirection;
    public float knockbackForce = 10f;
    private float knockbackTime;
    public float startKnockbackTime = 0.02f;
    private bool knockbackFlag;

    [Header("Damage Stats")]
    public Weapon weaponData;
    int damage = 1;
    bool criticalHitActivated;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.isTrigger = true;
        targetIsWithinRange = false;

        if (!dealsConstantDamage)
        {
            damageCollider.enabled = false;
        }

        if (weaponData != null)
        {
            knockbackForce = weaponData.knockbackForce;
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
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
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
        if (targetIsWithinRange)
        {
            knockbackTarget = collision.GetComponent<CharacterManager>();

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

                if (enemyStats != null)
                {
                    enemyStats.LoseHealth(damage, criticalHitActivated);
                    knockbackFlag = true;
                }
            }

            CharacterManager myCharacterManager = GetComponentInParent<CharacterManager>();
            if(myCharacterManager != null)
            {
                knockbackDirection = myCharacterManager.lastMoveDirection;
            }
            else
            {
                knockbackDirection = Vector2.zero;
            }


            yield return new WaitForFixedUpdate();
            StartCoroutine(DealDamage(collision));
        }
        else
            yield break;
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
        if (weaponData != null)
        {
            float randValue = Random.value;

            if (randValue < 1 - weaponData.criticalChance)
            {
                damage = Random.Range(weaponData.currentMinDamage, weaponData.currentMaxDamage);
                criticalHitActivated = false;
            }
            else
            {
                damage = weaponData.currentMaxDamage * 2;
                criticalHitActivated = true;
            }
            Debug.Log("Damage: " + damage);
        }
    }
}
