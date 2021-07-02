using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [Header("Vessel Stats")]
    public int vesselPoints;
    public float playerVesselPercentage;

    [Header("Completion IDs")]
    public List<int> completedBossIDs;
    public List<int> completedChestIDs;

    [Header("Phone Calls")]
    public List<Contact> collectedContacts;
    public List<int> collectedPhoneCallIDs;

    [Header("Exploration")]
    public int roomsDiscovered;
    public int lastRoomVisited;
    public int areasDiscovered;
    public int lastAreaVisited;

    private void Awake()
    {
        playerVesselPercentage = vesselPoints * 0.1f;
    }


    public void AdjustVesselLevel(int vesselPointAdjustment)
    {
        vesselPoints += vesselPointAdjustment;
        playerVesselPercentage = vesselPoints * 0.1f;
    }
}
