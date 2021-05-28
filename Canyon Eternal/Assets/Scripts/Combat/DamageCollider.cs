using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Tooltip("Assign if this damage collider belongs to a melee weapon")]
    public Card cardData;
    Collider2D damageCollider;

    int damage = 1;

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
            damage = cardData.damage;
        }
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
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.GetComponent<CharacterManager>() != null)
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

        yield return new WaitForFixedUpdate();
        StartCoroutine(DealDamage(collision));
    }
}
