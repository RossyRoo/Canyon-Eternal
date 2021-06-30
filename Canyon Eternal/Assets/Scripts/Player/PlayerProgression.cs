﻿using System.Collections;
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
