using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/ Attack Action")]
public class EnemyAttackAction : EnemyActions
{
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maximumAttackAngle = 45;
    public float minimumAttackAngle = -45;

    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

    public float chargeForce = 999f;

}
