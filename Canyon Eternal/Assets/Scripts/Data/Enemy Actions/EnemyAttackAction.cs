using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Action")]
public class EnemyAttackAction : EnemyActions
{
    [TextArea]
    public string comment;

    public float recoveryTime = 1f;

    public float spaceNeededToStartAttack= 0;
    public float shortestDistanceNeededToAttack = 10;

    public float prepareAttackTime = 0f;

    [Header("MELEE")]
    [Tooltip("Time between attack start and opening damage collider")]
    public float openDamageColliderBuffer = 0.1f;
    [Tooltip("Time between opening damage collider and closing damage collider")]
    public float closeDamageColliderBuffer = 0.6f;
    [Tooltip("The amount of force added to the enemy's charge. Negative force will knock the enemy back")]
    [Range(0,50000)]public float chargeForce = 999f;
    [Tooltip("The amount of time an enemy is charging toward its target")]
    public float chargeDuration = 0f;

    [Header("RANGED")]
    public bool isRanged;
    public StandardProjectile projectilePrefab;

}
