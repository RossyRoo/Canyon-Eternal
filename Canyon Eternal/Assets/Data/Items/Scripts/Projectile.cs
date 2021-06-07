using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Projectile")]
public class Projectile : Weapon
{
    [Header("PROJECTILE")]
    [Range(0,10)] public float explostionRadius;
    public GameObject GOPrefab;
    public float launchForce;
    public AudioClip launchSFX;
    public AudioClip collisionSFX;
    public GameObject collisionVFX;

}
