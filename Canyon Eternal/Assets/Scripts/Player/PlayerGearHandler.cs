using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGearHandler : MonoBehaviour
{
    PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();

        if (playerInventory.activeGear == null)
        {
            if (playerInventory.gearInventory.Count != 0)
            {
                playerInventory.activeGear = playerInventory.gearInventory[0];
            }
        }
    }
}
