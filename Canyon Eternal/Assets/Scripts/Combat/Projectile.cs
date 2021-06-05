using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    DamageCollider damageCollider;
    Rigidbody2D rb;

    public ItemSFXBank itemSFXBank;

    public float speed = 100f;
    public Vector2 direction;

    private void Awake()
    {

        damageCollider = GetComponent<DamageCollider>();
        rb = GetComponent<Rigidbody2D>();

        SFXPlayer.Instance.PlaySFXAudioClip(itemSFXBank.useItem, 0.1f);
    }

    private void Start()
    {
        damageCollider.EnableDamageCollider();
    }

    private void FixedUpdate()
    {
        FlyStraight();
    }

    private void FlyStraight()
    {
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
    }

    public void SetSpeedAndDirection(float newSpeed, Vector2 newDirection)
    {
        speed = newSpeed;
        direction = newDirection;
        damageCollider.knockbackDirection = direction;
    }
}
