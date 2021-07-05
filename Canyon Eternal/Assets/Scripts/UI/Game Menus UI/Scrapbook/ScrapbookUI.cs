using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrapbookUI : MonoBehaviour
{
    PlayerProgression playerProgression;
    PlayerInventory playerInventory;
    SceneChangeManager sceneChangeManager;

    public GameMenuUI gameMenuUI;
    public TextMeshProUGUI submenuNameText;
    public GameObject scrapbookUIGO;
    public GameObject mapUIGO;
    public GameObject journalUIGO;
    public GameObject bestiaryUIGO;

    [Header("World Map")]
    public GameObject playerIcon;
    public GameObject worldMapUIGO;
    public GameObject[] worldMapAreas;

    [Header("Area Maps")]
    public GameObject areaMapUIGO;
    public GameObject[] areaMaps;
    GameObject currentAreaMap;
    AreaSlot[] roomsInCurrentArea;

    [Header("Bestiary")]
    public GameObject beastiaryInfoPanel;
    public CharacterDataSlot[] beastiarySlots;
    public Image beastiaryIcon;
    public TextMeshProUGUI beastiaryName;
    public TextMeshProUGUI beastiaryDescription;

    [Header("Journal")]
    public GameObject entryInfoPanel;
    public ItemSlotUI[] entrySlots;
    public Image entryIcon;
    public TextMeshProUGUI entryDescription;
    public TextMeshProUGUI entryText;


    public void OpenScrapbook(int currentSubmenuIndex)
    {
        if (playerProgression == null)
        {
            playerProgression = FindObjectOfType<PlayerProgression>();
            playerInventory = FindObjectOfType<PlayerInventory>();
            sceneChangeManager = FindObjectOfType<SceneChangeManager>();
        }

        gameMenuUI.menuNameText.text = "Scrapbook";
        scrapbookUIGO.SetActive(true);

        if(currentSubmenuIndex == 0)
        {
            OpenMap();
        }
        else if(currentSubmenuIndex == 1)
        {
            OpenJournal();
        }
        else
        {
            OpenBestiary();
        }
    }

    #region Map
    public void OpenMap()
    {
        submenuNameText.text = "";
        bestiaryUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        mapUIGO.SetActive(true);

        PrepareWorldMap();
    }


    public void PrepareWorldMap()
    {
        areaMapUIGO.SetActive(false);
        worldMapUIGO.SetActive(true);

        for (int i = 0; i < worldMapAreas.Length; i++)
        {
            AreaSlot thisArea = worldMapAreas[i].GetComponent<AreaSlot>();

            if(playerProgression.roomsDiscovered.Contains(thisArea.slotRoom))
            {
                if (thisArea.roomsInThisArea.Contains(sceneChangeManager.currentRoom))
                {
                    playerIcon.transform.position = thisArea.playerIconTF.position;
                }

                if (thisArea.areaCoverGO != null)
                {
                    Animator[] coverAnimators = thisArea.areaCoverGO.GetComponentsInChildren<Animator>();
                    for (int j = 0; j < coverAnimators.Length; j++)
                    {
                        coverAnimators[j].Play("RemoveCover");
                    }

                    Destroy(thisArea.areaCoverGO, 1);
                }
            }
        }
    }

    public void PrepareAreaMap()
    {
        worldMapUIGO.SetActive(false);
        areaMapUIGO.SetActive(true);

        for (int i = 0; i < roomsInCurrentArea.Length; i++)
        {
            AreaSlot thisRoom = roomsInCurrentArea[i].GetComponent<AreaSlot>();

            if (playerProgression.roomsDiscovered.Contains(thisRoom.slotRoom))
            {
                if (sceneChangeManager.currentRoom == thisRoom.slotRoom)
                {
                    playerIcon.SetActive(true);
                    playerIcon.transform.position = thisRoom.playerIconTF.position;
                }

                if (thisRoom.areaCoverGO != null)
                {
                    Animator[] coverAnimators = thisRoom.areaCoverGO.GetComponentsInChildren<Animator>();
                    for (int j = 0; j < coverAnimators.Length; j++)
                    {
                        coverAnimators[j].Play("RemoveCover");
                    }

                    Destroy(thisRoom.areaCoverGO, 1);
                }
            }
        }
    }

    public void SwitchToAreaMap(GameObject areaMap)
    {
        playerIcon.SetActive(false);

        areaMap.SetActive(true);
        currentAreaMap = areaMap;

        roomsInCurrentArea = currentAreaMap.GetComponentsInChildren<AreaSlot>();

        PrepareAreaMap();
    }

    public void SwitchToWorldMap()
    {
        playerIcon.SetActive(true);

        for (int i = 0; i < areaMaps.Length; i++)
        {
            areaMaps[i].SetActive(false);
        }
        PrepareWorldMap();
    }

    #endregion

    #region Beastiary
    public void OpenBestiary()
    {
        submenuNameText.text = "Bestiary";
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        bestiaryUIGO.SetActive(true);

        for (int i = 0; i < beastiarySlots.Length; i++)
        {
            if (i < playerProgression.enemiesEncountered.Count)
            {
                beastiarySlots[i].GetComponent<CharacterDataSlot>().slotCharacterData = playerProgression.enemiesEncountered[i];
                beastiarySlots[i].GetComponent<Image>().sprite = playerProgression.enemiesEncountered[i].characterIcon;
            }
        }
    }

    public void SelectEnemyToDisplay(CharacterDataSlot characterDataSlot)
    {
        CharacterData characterDataToDisplay = characterDataSlot.slotCharacterData;

        if(characterDataToDisplay != null)
        {
            beastiaryInfoPanel.SetActive(true);
            beastiaryName.text = characterDataToDisplay.characterName;
            beastiaryDescription.text = characterDataToDisplay.characterDescription;
            beastiaryIcon.sprite = characterDataToDisplay.characterIcon;
        }
    }
    #endregion

    #region Journal

    public void OpenJournal()
    {
        submenuNameText.text = "Journal";
        bestiaryUIGO.SetActive(false);
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(true);

        for (int i = 0; i < entrySlots.Length; i++)
        {
            if(i < playerInventory.loreEntryInventory.Count)
            {
                entrySlots[i].GetComponent<ItemSlotUI>().slotItem = playerInventory.loreEntryInventory[i];
                entrySlots[i].GetComponent<Image>().sprite = playerInventory.loreEntryInventory[i].itemIcon;
            }
        }
    }

    public void SelectEntryToDisplay(ItemSlotUI itemSlotUI)
    {
        Item entryToDisplay = itemSlotUI.slotItem;

        if(entryToDisplay != null)
        {
            entryInfoPanel.SetActive(true);
            entryDescription.text = entryToDisplay.itemName;
            entryText.text = entryToDisplay.itemDescription;
            entryIcon.sprite = entryToDisplay.itemIcon;
        }
    }

    #endregion
}
