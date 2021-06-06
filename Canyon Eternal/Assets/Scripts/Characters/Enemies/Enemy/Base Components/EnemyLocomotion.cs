using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyParticleHandler enemyParticleHandler;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyParticleHandler = GetComponentInChildren<EnemyParticleHandler>();
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            if (!enemyManager.isFalling)
            {
                StartCoroutine(HandleFalling());
            }
        }
    }

    private IEnumerator HandleFalling()
    {
        enemyManager.isFalling = true;

        yield return new WaitForSeconds(0.4f);

        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        InvokeRepeating("ApplyFallForce", 0.4f, 0.0001f);

        yield return new WaitForSeconds(0.45f);
    }

    private void ApplyFallForce()
    {
        enemyParticleHandler.SpawnBigDustCloudVFX();
        enemyManager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyManager.rb.AddForce(Vector2.down * 20000f);
        enemyManager.isDead = true;
    }*/
}
