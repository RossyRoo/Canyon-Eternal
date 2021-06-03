using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Action")]
public class EnemyAttackAction : EnemyActions
{
    public int attackScore = 3;
    public float recoveryTime = 1f;

    public float spaceNeededToStartAttack= 3;
    public float shortestDistanceNeededToAttack = 5;

    [Range(1,100)]public float chargeForce = 999f;

}
