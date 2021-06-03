using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //BackpackMenu backpackMenu;

    public int fragmentInventory;


    public MeleeWeapon heldThrustCard;
    public MeleeWeapon heldSlashCard;
    public MeleeWeapon heldStrikeCard;


    private void Awake()
    {
        //backpackMenu = FindObjectOfType<BackpackMenu>();
    }


    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;
        //backpackMenu.UpdateFragmentCount();
    }
}
