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

    public bool isDead;
    public bool isInteracting;
    public bool isPerformingAction;
    public bool isInvulnerable;
    public bool isMoving;

    [Header("Combat States")]
    public bool isAttacking;
    public bool isBlocking;
    public bool isVulnerableToBlock;
    public bool isStunned;
    public bool isDashing;

    public Vector2 moveDirection;
    public Vector2 lastMoveDirection = Vector2.down;

    public float stunTime = 2f;
    public float deathTimeBuffer = 0.5f;

    public void GenerateTrackingWall()
    {
        myWall = Instantiate<TrackingWall>(myWallPrefab);
        myWall.Init(transform);
        myWall.transform.parent = GameObject.FindGameObjectWithTag("Tracking Walls").transform;
    }

}
