using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [Header("Vessel Stats")]
    public int vesselPoints;
    public float playerVesselPercentage;

    [Header("Completion IDs")]
    public List<CharacterData> enemiesEncountered;
    public List<int> collectedChestIDs;

    [Header("Phone Calls")]
    public List<Contact> collectedContacts;
    public List<int> collectedPhoneCallIDs;

    [Header("Exploration")]
    public List<Room> roomsDiscovered;
    public List<Room> fastTravelLocationsDiscovered;


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
