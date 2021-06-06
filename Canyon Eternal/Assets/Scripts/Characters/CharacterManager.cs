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
    public bool isLockedInPlace;

    [Header("Combat")]
    public bool isAttacking;
    public bool isChargingSpell;
    public bool isCastingSpell;
    public bool isBlocking;
    public bool isVulnerableToBlock;
    public bool isStunned;
    public bool isDashing;

    public Vector2 currentMoveDirection;
    public Vector2 lastMoveDirection;

    public void GenerateTrackingWall()
    {
        myWall = Instantiate<TrackingWall>(myWallPrefab);
        myWall.Init(transform);
        myWall.transform.parent = GameObject.FindGameObjectWithTag("Tracking Walls").transform;
    }
}
