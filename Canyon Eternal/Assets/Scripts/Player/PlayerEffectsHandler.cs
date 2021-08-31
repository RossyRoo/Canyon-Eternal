using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour
{
    public void UseItem(Item item)
    {
        if(item.dataName == "Potion")
        {
            Debug.Log("Using POTION");
        }
        else if (item.dataName == "Cure")
        {
            Debug.Log("Using CURE");
        }
        else if(item.dataName == "Tent")
        {
            Debug.Log("Using TENT");
        }
        else if (item.dataName == "Coffee")
        {
            Debug.Log("Using COFFEE");

        }
        else if(item.dataName == "Smoke Bomb")
        {
            Debug.Log("Using SMOKE BOMB");

        }
    }
}
