using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //BackpackMenu backpackMenu;

    public int fragmentInventory;

    private void Awake()
    {
        //backpackMenu = FindObjectOfType<BackpackMenu>();
    }

    public List<Card> cardInventory = new List<Card>();
    //public List<Advantage> advantageInventory = new List<Advantage>();
    //public List<Treasure> treasureInventory = new List<Treasure>();
    //public List<Key> keyInventory = new List<Key>();

    //public List<JournalEntry> journalEntryInventory = new List<JournalEntry>();

    public void AdjustFragmentInventory(int adjustment)
    {
        fragmentInventory += adjustment;
        //backpackMenu.UpdateFragmentCount();
    }
}
