using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneCall : MonoBehaviour
{
    public int phoneCallID;
    public int minVesselPercentage = 0;
    public int maxVesselPercentage = 100;
    public int bossIDRequirement = 0;

    private void Awake()
    {
        Debug.Log("Phone call Game Object Spawned");
    }

}
