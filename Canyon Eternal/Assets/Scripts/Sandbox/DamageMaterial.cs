using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMaterial : MonoBehaviour
{
    Collider2D damageCollider;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.LoseHealth(1);
            }
        }
    }
}
