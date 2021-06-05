using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    DamageCollider damageCollider;
    Rigidbody2D rb;

    public ItemSFXBank itemSFXBank;

    public bool isHeld;
    public float speed = 100f;
    public Vector2 direction;

    private void Awake()
    {
        damageCollider = GetComponent<DamageCollider>();
        rb = GetComponent<Rigidbody2D>();

        if(GetComponentInParent<PlayerManager>())
        {
            isHeld = true;
        }
    }


    private void FixedUpdate()
    {
        if(!isHeld)
        {
            FlyStraight();
        }
    }

    private void FlyStraight()
    {
        damageCollider.EnableDamageCollider();
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform == transform.parent.transform)
        {
            return;
        }

        if (collision.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }

        //Play Collision SFX
    }

    public void LaunchProjectileAsSpell(float newSpeed, Vector2 newDirection)
    {
        speed = newSpeed;
        direction = newDirection;
        damageCollider.knockbackDirection = direction;
        isHeld = false;
        transform.parent = FindObjectOfType<ObjectPool>().transform;
    }
}
