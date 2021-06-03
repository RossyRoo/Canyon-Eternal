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

    [Header("Character States")]
    public bool isDead;
    public bool isInteracting;
    public bool isInvulnerable;
    public bool isMoving;
    public bool isAttacking;
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
