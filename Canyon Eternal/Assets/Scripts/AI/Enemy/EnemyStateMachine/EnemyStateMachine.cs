using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    public abstract EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler);
}