﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Projectile")]
public class Projectile : Weapon
{
    [Header("PROJECTILE PARAMETERS")]
    [Range(0,10)] public float explosionRadius;
    public float launchForce;

    public GameObject GOPrefab;

    public AudioClip launchSFX;
    public AudioClip collisionSFX;
}
