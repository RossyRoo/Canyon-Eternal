using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    PlayerManager playerManager;
    float searchDistance = 45f;
    public LayerMask enemyLayer;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }





}
