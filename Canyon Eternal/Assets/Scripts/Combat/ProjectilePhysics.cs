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

    private void Awake()
    {
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
        if (isFired)
        {
            if(projectileData.explostionRadius != 0)
            {
                GetComponent<CircleCollider2D>().radius = projectileData.explostionRadius;
            }

            SFXPlayer.Instance.PlaySFXAudioClip(projectileData.collisionSFX);
            SpawnCollisionVFX();

            Destroy(gameObject, 0.1f);
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
        Debug.Log("Spawning Col FX");
        GameObject collisionVFXGO = Instantiate(projectileData.collisionVFX, transform.position, Quaternion.identity);
        Destroy(collisionVFXGO, 3f);
    }
}
