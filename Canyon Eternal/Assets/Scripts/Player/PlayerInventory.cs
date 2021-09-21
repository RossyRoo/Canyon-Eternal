using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    FragmentCounterUI fragmentCounterUI;

    public int fragmentInventory;

    [Header("Melee Inventory")]
    public MeleeWeapon activeWeapon;
    public List<MeleeWeapon> weaponsInventory;

    [Header("Spell Inventory")]
    public Spell activeSpell;
    public List<Spell> spellsInventory;

    [Header("Gear Inventory")]
    public Gear activeGear;
    public List<Gear> gearInventory;

    [Header("Offhand Inventory")]
    public OffhandWeapon activeOffhandWeapon;
    public List<OffhandWeapon> offhandWeaponInventory;

    [Header("Item Inventory")]
    public Consumable activeConsumable;
    public List<Item> itemInventory;

    [Header("Photo Inventory")]
    public List<Photo> photoInventory;

    [Header("Artifact Entry Collection")]
    public List<DataObject> artifactInventory;


    private void Awake()
    {
        fragmentCounterUI = FindObjectOfType<FragmentCounterUI>();
    }

    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;

        if(adjustment >= 0)
        {
            StartCoroutine(fragmentCounterUI.IncreaseFragmentCountUI(adjustment));
        }
        else
        {
            StartCoroutine(fragmentCounterUI.DecreaseFragmentCountUI(adjustment));
        }
    }
}
