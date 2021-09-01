using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    DamageCollider damageCollider;
    Rigidbody2D rb;

    [Header("Projectile Data Slot")]
    public StandardProjectile standardProjectileData;
    public Spell spellProjectileData;

    //MY PARAMETERS
    float myLaunchForce = 0;
    float myExplosionRadius;
    GameObject myCollisionVFX;
    AudioClip myCollisionSFX;
    public bool isEnemyProjectile;

    bool isFired;
    Vector2 direction;


    private void Awake()
    {
        DetermineProjectileData();

        damageCollider = GetComponent<DamageCollider>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isFired)
        {
            FlyStraight();
        }
    }

    private void DetermineProjectileData()
    {
        if (GetComponentInParent<EnemyManager>())
        {
            isEnemyProjectile = true;
        }

        if (standardProjectileData != null)
        {
            myLaunchForce = standardProjectileData.launchForce;
            myExplosionRadius = standardProjectileData.explosionRadius;
            myCollisionVFX = standardProjectileData.collisionVFX;
            myCollisionSFX = standardProjectileData.collisionSFX;
        }
        else if (spellProjectileData != null)
        {
            myLaunchForce = spellProjectileData.launchForce;
            myExplosionRadius = spellProjectileData.explosionRadius;
            myCollisionVFX = spellProjectileData.collisionVFX;
            myCollisionSFX = spellProjectileData.collisionSFX;
        }
    }

    private void FlyStraight()
    {
        damageCollider.EnableDamageCollider();
        rb.velocity = direction * myLaunchForce;
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEnemyProjectile && collision.GetComponent<EnemyManager>())
        {
            return;
        }
        else if(!isEnemyProjectile && collision.GetComponent<PlayerManager>())
        {
            return;
        }
        else
        {
            if (isFired)
            {
                isFired = false;
                if (myExplosionRadius != 0)
                {
                    GetComponent<CircleCollider2D>().radius = myExplosionRadius;
                }

                SpawnCollisionVFX();
                SFXPlayer.Instance.PlaySFXAudioClip(myCollisionSFX);

                Destroy(gameObject);
            }
        }
    }

    public void Launch(float newLaunchForce, Vector2 newDirection)
    {
        myLaunchForce = newLaunchForce;
        direction = newDirection;
        damageCollider.knockbackDirection = direction;
        isFired = true;
        transform.parent = FindObjectOfType<ObjectPool>().transform;
    }

    private void SpawnCollisionVFX()
    {
        if(myCollisionVFX != null)
        {
            GameObject collisionVFXGO = Instantiate(myCollisionVFX, transform.position, Quaternion.identity);
            Destroy(collisionVFXGO, 3f);
        }

    }
}
