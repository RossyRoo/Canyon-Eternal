using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public FragmentCounterUI fragmentCounterUI;

    public int fragmentInventory;

    public MeleeWeapon heldThrustCard;
    public MeleeWeapon heldSlashCard;
    public MeleeWeapon heldStrikeCard;


    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;
        StartCoroutine(fragmentCounterUI.UpdateFragmentCountUI(adjustment));
    }
}
