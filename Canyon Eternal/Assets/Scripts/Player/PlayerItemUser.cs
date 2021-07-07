using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUser : MonoBehaviour
{
    PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }
    /*
    public void UseItemFromQuickSlot()
    {
        Debug.Log("Using quick slot usable");
        playerInventory.usableInventory.Remove(playerInventory.quickSlotUsable);
    }*/

    public void UseItemFromInventory(Usable itemToUse)
    {
        Debug.Log("Using inventory usable");
        playerInventory.usableInventory.Remove(itemToUse);

    }

}
