using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public StandardProjectile projectile;
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
            GameObject projectileGO = Instantiate(projectile.GOPrefab, firingPoint.position, firingPoint.rotation);
            projectileGO.GetComponent<ProjectilePhysics>().Launch(projectile.launchForce, transform.up);

            SFXPlayer.Instance.PlaySFXAudioClip(projectile.launchSFX, 0.05f);
        }

        StartCoroutine(FireProjectileCoroutine());
    }
}
