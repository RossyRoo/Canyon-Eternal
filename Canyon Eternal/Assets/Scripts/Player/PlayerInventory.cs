using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    FragmentCounterUI fragmentCounterUI;

    [HideInInspector]
    public int fragmentInventory;

    [Header("Melee Inventory")]
    public List<MeleeWeapon> weaponsInventory;

    [Header("Spell Inventory")]
    public List<Spell> spellsInventory;

    [Header("Item Inventory")]
    public List<Item> itemInventory;

    [Header("Photo Inventory")]
    public List<Photo> photoInventory;

    [Header("Lore Entry Collection")]
    public List<DataObject> artifactInventory;


    private void Awake()
    {
        fragmentCounterUI = FindObjectOfType<FragmentCounterUI>();
    }

    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;
        StartCoroutine(fragmentCounterUI.UpdateFragmentCountUI(adjustment));
    }
}
