using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Action")]
public class EnemyAttackAction : EnemyActions
{
    [TextArea]
    public string comment;

    public int attackScore = 3;
    public float recoveryTime = 1f;

    public float spaceNeededToStartAttack= 0;
    public float shortestDistanceNeededToAttack = 10;

    [Header("Melee Settings")]
    [Tooltip("Time between attack start and opening damage collider")]
    public float openDamageColliderBuffer = 0.1f;
    [Tooltip("Time between opening damage collider and closing damage collider")]
    public float closeDamageColliderBuffer = 0.6f;

    [Range(1,100)]public float chargeForce = 999f;

    [Header("Ranged Settings")]
    public bool isRanged;

}
