using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    FragmentCounterUI fragmentCounterUI;

    [HideInInspector]
    public int fragmentInventory;

    [Header("Melee Inventory")]
    public List<MeleeWeapon> thrustWeaponsInventory;
    public List<MeleeWeapon> slashWeaponsInventory;
    public List<MeleeWeapon> strikeWeaponsInventory;

    [Header("Spell Inventory")]
    public List<Spell> projectileSpellsInventory;
    public List<Spell> aOESpellsInventory;
    public List<Spell> buffSpellsInventory;

    [Header("Item Inventory")]
    public List<Key> keyInventory;
    public List<Treasure> treasureInventory;
    public List<Usable> usableInventory;



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
