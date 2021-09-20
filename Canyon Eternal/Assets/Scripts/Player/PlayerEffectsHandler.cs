using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour
{
    public void UseConsumable(Consumable consumable)
    {
        SFXPlayer.Instance.PlaySFXAudioClip(consumable.consumeSFX);
        Debug.Log("USING AN ITEM");

        if(consumable.dataName == "Lo Potion")
        {
            Debug.Log("Using POTION");
        }
        else if (consumable.dataName == "Cure")
        {
            Debug.Log("Using CURE");
        }
        else if(consumable.dataName == "Tent")
        {
            Debug.Log("Using TENT");
        }
        else if (consumable.dataName == "Coffee")
        {
            Debug.Log("Using COFFEE");

        }
        else if(consumable.dataName == "Smoke Bomb")
        {
            Debug.Log("Using SMOKE BOMB");

        }
    }
}
