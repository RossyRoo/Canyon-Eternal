using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //BackpackMenu backpackMenu;

    public int fragmentInventory;


    public MeleeCard heldThrustCard;
    public MeleeCard heldSlashCard;
    public MeleeCard heldStrikeCard;


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
