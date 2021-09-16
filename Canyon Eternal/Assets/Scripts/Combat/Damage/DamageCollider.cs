using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    ObjectPool objectPool;
    Collider2D damageCollider;
    float damage = 1;
    bool criticalHitActivated;
    //KNOCKBACK PARAMETERS
    CharacterManager currentTarget;
    float knockbackForce;
    float knockbackTime;
    float startKnockbackTime = 0.04f;
    bool knockbackFlag;

    [Header("Collider Parameters")]
    public Weapon weaponData;
    public Transform collisionTransform;
    public bool dealsConstantDamage;
    public bool targetIsWithinRange = false;
    public bool canDamageEnemy = true;
    public bool canDamagePlayer = true;


    private void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        damageCollider = GetComponent<Collider2D>();

        knockbackTime = startKnockbackTime;
        knockbackForce = weaponData.knockbackForce;
        weaponData.minDamage = weaponData.startingMinDamage;
        weaponData.maxDamage = weaponData.startingMaxDamage;

        if (!dealsConstantDamage)
        {
            damageCollider.enabled = false;
        }
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
        currentTarget = collision.gameObject.GetComponent<CharacterManager>();

        if (currentTarget != null)
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
            if (currentTarget != null && !currentTarget.isInvulnerable)
            {
                if (collision.tag == "Player" && canDamagePlayer)
                {
                    knockbackFlag = true;
                    DetermineCriticalHit();
                    CalculateElementalDamage();
                    collision.GetComponent<PlayerStats>().LoseHealth(damage, true);
                }

                if (collision.tag == "Enemy" && canDamageEnemy)
                {
                    knockbackFlag = true;
                    DetermineCriticalHit();
                    CalculateElementalDamage();
                    collision.GetComponentInParent<EnemyStats>().LoseHealth(damage, criticalHitActivated);
                }
            }

            yield return new WaitForFixedUpdate();
            StartCoroutine(DealDamage(collision));
        }
        else
        {
            yield break;
        }
    }

    private void HandleKnockback()
    {
        if (knockbackFlag)
        {
            if (knockbackTime <= 0)
            {
                knockbackFlag = false;
                currentTarget = null;
                knockbackTime = startKnockbackTime;
            }
            else
            {
                knockbackTime -= Time.deltaTime;
                if(currentTarget != null)
                {
                    currentTarget.rb.AddForce(-currentTarget.transform.up * knockbackForce);
                }

                if (GetComponentInParent<PlayerManager>())
                {
                    PlayerManager playerManager = GetComponentInParent<PlayerManager>();
                    playerManager.rb.AddForce(-playerManager.transform.up * knockbackForce * 10);
                }
            }
        }
    }

    #region Damage and Status Calculation

    private void DetermineCriticalHit()
    {
        float randValue = Random.value;

        if (randValue < 1 - weaponData.criticalChance)
        {
            damage = Mathf.RoundToInt(Random.Range(weaponData.minDamage, weaponData.maxDamage));
            SFXPlayer.Instance.PlaySFXAudioClip(weaponData.damageSFX[Random.Range(0, weaponData.damageSFX.Length)], 0.05f);
            criticalHitActivated = false;
        }
        else
        {
            damage = Mathf.RoundToInt(weaponData.maxDamage * 2);
            SFXPlayer.Instance.PlaySFXAudioClip(weaponData.criticalDamageSFX);
            criticalHitActivated = true;
        }
    }

    private void CalculateElementalDamage()
    {
        float randValue = Random.value;

        float elementalBoost = 0;

        if (weaponData.damageType > 0)
        {
            if (FindObjectOfType<WeatherManager>().currentPattern == weaponData.damageType)
            {
                elementalBoost += 0.2f;
            }

            if (randValue < 0.2 + elementalBoost)
            {
                damage += weaponData.minDamage;
                GameObject collisionVFXGO = Instantiate(weaponData.collisionVFX[weaponData.damageType], collisionTransform.position, Quaternion.identity);
                collisionVFXGO.transform.parent = objectPool.transform;
                Destroy(collisionVFXGO, 1f);
            }
            else
            {
                GameObject collisionVFXGO = Instantiate(weaponData.collisionVFX[weaponData.damageType], collisionTransform.position, Quaternion.identity);
                collisionVFXGO.transform.parent = objectPool.transform;
                Destroy(collisionVFXGO, 1f);
            }
        }
    }

    #endregion
}
