using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public Card cardData;

    Collider2D damageCollider;
    int currentWeaponDamage;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;

        currentWeaponDamage = cardData.damage;
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
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.LoseHealth(currentWeaponDamage);
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();

            if (enemyStats != null && !enemyManager.isDead)
            {
                enemyStats.LoseHealth(currentWeaponDamage);
            }
        }
    }
}
