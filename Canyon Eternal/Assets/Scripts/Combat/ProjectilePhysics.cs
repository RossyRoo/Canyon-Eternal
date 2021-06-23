using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    DamageCollider damageCollider;
    Rigidbody2D rb;

    public Projectile projectileData;

    public bool isFired;
    public float speed;
    public Vector2 direction;
    bool isEnemyProjectile;

    private void Awake()
    {
        if(GetComponentInParent<EnemyManager>())
        {
            isEnemyProjectile = true;
        }
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

    private void FlyStraight()
    {
        damageCollider.EnableDamageCollider();
        rb.velocity = direction * speed;
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
                if (projectileData.explosionRadius != 0)
                {
                    GetComponent<CircleCollider2D>().radius = projectileData.explosionRadius;
                }

                SpawnCollisionVFX();
                SFXPlayer.Instance.PlaySFXAudioClip(projectileData.collisionSFX);
                Debug.Log("Spawning Collision FX");

                Destroy(gameObject);
            }
        }
    }

    public void Launch(float newSpeed, Vector2 newDirection)
    {
        speed = newSpeed;
        direction = newDirection;
        damageCollider.knockbackDirection = direction;
        isFired = true;
        transform.parent = FindObjectOfType<ObjectPool>().transform;
    }

    private void SpawnCollisionVFX()
    {
        if(projectileData.collisionVFX != null)
        {
            GameObject collisionVFXGO = Instantiate(projectileData.collisionVFX, transform.position, Quaternion.identity);
            Destroy(collisionVFXGO, 3f);
        }

    }
}
