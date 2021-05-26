using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firingPoint;
    public float rateOfFireOffset = 0f;
    public float rateOfFire = 1f;
    public bool canFire;

    private void Start()
    {
        canFire = true;
        StartCoroutine(WaitForInitialProjectileDelay());
    }

    private IEnumerator WaitForInitialProjectileDelay()
    {
        yield return new WaitForSeconds(rateOfFireOffset);
        StartCoroutine(FireProjectileCoroutine());
    }

    private IEnumerator FireProjectileCoroutine()
    {
        yield return new WaitForSeconds(rateOfFire);

        if(canFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            projectile.transform.parent = gameObject.transform;
            Destroy(projectile, 5f);
        }

        StartCoroutine(FireProjectileCoroutine());
    }
}
