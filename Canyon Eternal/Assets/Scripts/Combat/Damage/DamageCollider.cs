using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    ObjectPool objectPool;

    [HideInInspector]public Collider2D damageCollider;
    public Transform collisionTransform;
    public CharacterManager myCharacterManager;

    [Header("Collider Type")]
    public bool dealsConstantDamage;
    public bool targetIsWithinRange;
    public bool canDamageEnemy = true;
    public bool canDamagePlayer = true;

    [Header("Knockback Settings")]
    public CharacterManager knockbackTarget;
    public float knockbackForce = 10f;
    private float knockbackTime;
    public float startKnockbackTime = 0.02f;
    private bool knockbackFlag;

    [Header("Damage Stats")]
    public Weapon weaponData;
    float damage = 1;
    bool criticalHitActivated;

    private void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
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
            weaponData.minDamage = weaponData.startingMinDamage;
            weaponData.maxDamage = weaponData.startingMaxDamage;
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
            knockbackTarget = characterCollision;
            targetIsWithinRange = true;
            StartCoroutine(DealDamage(collision.gameObject));
        }

        if(collision.gameObject.transform != transform.parent
            && weaponData != null
            && weaponData.collisionVFX != null)
        {
            GameObject collisionVFXGO = Instantiate(weaponData.collisionVFX, collisionTransform.position, Quaternion.identity);
            collisionVFXGO.transform.parent = objectPool.transform;
            Destroy(collisionVFXGO, 1f);
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
            if (knockbackTarget.isInvulnerable)
            {
                yield break;
            }

            RollForCriticalHit();

            if (collision.tag == "Player" && canDamagePlayer)
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

                CinemachineManager.Instance.Shake(7f, 0.25f);
            }

            if (weaponData.criticalDamageSFX != null)
            {
                SFXPlayer.Instance.PlaySFXAudioClip(weaponData.damageSFX[Random.Range(0, weaponData.damageSFX.Length)], 0.1f);

                if (criticalHitActivated)
                {
                    SFXPlayer.Instance.PlaySFXAudioClip(weaponData.criticalDamageSFX);
                }
            }


            myCharacterManager = GetComponentInParent<CharacterManager>();

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
                knockbackTarget.rb.AddForce(-knockbackTarget.transform.up * knockbackForce);

                if(myCharacterManager.GetComponent<PlayerManager>())
                {
                    myCharacterManager.rb.AddForce(-myCharacterManager.transform.up * knockbackForce * 10);
                }
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
                damage = Mathf.RoundToInt(Random.Range(weaponData.minDamage, weaponData.maxDamage));
                criticalHitActivated = false;
            }
            else
            {
                damage = Mathf.RoundToInt(weaponData.maxDamage * 2);
                criticalHitActivated = true;
            }
        }
    }
}
