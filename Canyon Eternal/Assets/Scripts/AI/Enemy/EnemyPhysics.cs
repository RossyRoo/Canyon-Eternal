using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics.IgnoreLayerCollision(9, 8);
    }

    void Start()
    {

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        //Output the name of the GameObject you collide with
        Debug.Log("I hit the GameObject : " + collision.gameObject.name);
    }
}
