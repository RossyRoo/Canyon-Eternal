using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleHandler : MonoBehaviour
{
    [Header("Transforms")]
    public Transform overheadParticleTransform;
    public Transform torsoParticleTransform;
    public Transform feetParticleTransform;

    public GameObject bigDustVFX;

    public void SpawnBigDustCloudVFX()
    {
        GameObject dashParticleVFXGO = Instantiate(bigDustVFX, feetParticleTransform.position, Quaternion.identity);
        dashParticleVFXGO.transform.parent = null;
        Destroy(dashParticleVFXGO, dashParticleVFXGO.GetComponent<ParticleSystem>().main.duration);
    }
}
