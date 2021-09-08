using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public TrackingWall myWallPrefab;
    [HideInInspector]
    public TrackingWall myWall;

    [HideInInspector]
    public Rigidbody2D rb;

    [Header("CHARACTER BOOLS")]

    [Header("General")]
    public bool isDead;
    public bool isInteracting;
    public bool isInvulnerable;
    public bool isMoving;
    public bool isFalling;
    public bool isConversing;
    public bool isInCombat;

    [Header("Combat")]
    public bool isAttacking;
    public bool isChargingSpell;
    public bool isCastingSpell;
    public bool isBlocking;
    public bool isVulnerableToBlock;
    public bool isStunned;
    public bool isDashing;

    [Header("Movement")]
    public Vector2 currentMoveDirection;
    public Vector2 lastMoveDirection;

    public void GenerateTrackingWall()
    {
        myWall = Instantiate<TrackingWall>(myWallPrefab);
        myWall.Init(transform);

        DontDestroy[] dontDestroys = FindObjectsOfType<DontDestroy>();

        for (int i = 0; i < dontDestroys.Length; i++)
        {
            if(dontDestroys[i].isPersistent)
            {
                ObjectPool myObjectPool = dontDestroys[i].GetComponentInChildren<ObjectPool>();

                myWall.transform.parent = myObjectPool.transform;
            }
        }

    }
}
