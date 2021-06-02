﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public TrackingWall myWallPrefab;
    [HideInInspector]
    public TrackingWall myWall;

    [HideInInspector]
    public Rigidbody2D rb;

    public bool isInteracting;
    public bool isInvulnerable;
    public bool isAttacking;
    public bool isDead;
    public bool isMoving;
    public bool isBlocking;
    //public bool isVulnerableToBlock;

    public Vector2 moveDirection;
    public Vector2 lastMoveDirection = Vector2.down;

    public bool isConversing; //Used when conversation is happening on this character
    public bool isPerformingAction; //Character actions like performing attacks, healing, or evading

    public float deathTimeBuffer = 0.5f;

    public void GenerateTrackingWall()
    {
        myWall = Instantiate<TrackingWall>(myWallPrefab);
        myWall.Init(transform);
        myWall.transform.parent = GameObject.FindGameObjectWithTag("Tracking Walls").transform;
    }

}
