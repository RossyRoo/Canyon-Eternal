using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public TrackingWall myWallPrefab;

    public bool isInteracting;
    public bool isInvulnerable;
    public bool isDead;

    public bool isConversing; //Used when conversation is happening on this character
    public bool isPerformingAction; //Character actions like performing attacks, healing, or evading

    public void GenerateTrackingWall()
    {
        TrackingWall myWall = Instantiate<TrackingWall>(myWallPrefab);
        myWall.Init(transform);
        myWall.transform.parent = FindObjectOfType<TrackingWallPool>().gameObject.transform;
    }
}
