using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerSpellHandler playerSpellHandler;

    [Header("Interface Panel")]
    public Sprite emptyWindowSprite;
    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public int interfacePageIndex = 0;

    [Header("Artifacts")]
    public DataSlotUI[] artifactSlots;

    [Header("Gear")]
    public GameObject equipmentGrid;
    public GameObject equipButton;
    public GameObject gearSelectionOverview;
    public GameObject gearSelectionOverviewButton;
    public GameObject currentSpellButton;
    public GameObject currentWeaponButton;
    public GameObject[] equipmentSlots;


    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerSpellHandler = FindObjectOfType<PlayerSpellHandler>();
        playerMeleeHandler = FindObjectOfType<PlayerMeleeHandler>();

        gameMenuUI.menuNameText.text = "Bag";

        for (int i = 0; i < interfacePages.Length; i++)
        {
            if (i == 0)
            {
                interfacePages[i].SetActive(true);
            }
            else
            {
                interfacePages[i].SetActive(false);
            }
        }

        if (currentSubmenuIndex == 0)
        {
            OpenEquipment();
        }
        else if (currentSubmenuIndex == 1)
        {
            OpenItemInventory();
        }
        else
        {
            OpenArtifacts();
        }
    }

    public void CloseBag()
    {
        gameMenuUI.inventoryUIGO.SetActive(false);
        gameMenuUI.gearUIGO.SetActive(false);
    }

    #region Open Inventory


    public void OpenItemInventory()
    {
        gameMenuUI.submenuNameText.text = "Inventory";
        gameMenuUI.gearUIGO.SetActive(false);
        gameMenuUI.inventoryUIGO.SetActive(true);


        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = interfaceSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.itemInventory.Count)
            {
                itemSlotUI.slotData = playerInventory.itemInventory[i];

                myItemIcon.sprite = playerInventory.itemInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotData = null;
                myItemIcon.sprite = emptyWindowSprite;
            }
        }
    }

    #endregion

    public void CycleInterfacePages()
    {
        if (interfacePageIndex < interfacePages.Length - 1)
        {
            interfacePageIndex++;
        }
        else
        {
            interfacePageIndex = 0;
        }

        for (int i = 0; i < interfacePages.Length; i++)
        {
            if (i == interfacePageIndex)
            {
                interfacePages[i].SetActive(true);
            }
            else
            {
                interfacePages[i].SetActive(false);
            }
        }

        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);
    }

    #region Artifacts

    public void OpenArtifacts()
    {
        gameMenuUI.submenuNameText.text = "Artifacts";
        gameMenuUI.gearUIGO.SetActive(false);
        gameMenuUI.inventoryUIGO.SetActive(true);

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = interfaceSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.artifactInventory.Count)
            {
                itemSlotUI.slotData = playerInventory.artifactInventory[i];

                myItemIcon.sprite = playerInventory.artifactInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotData = null;
                myItemIcon.sprite = emptyWindowSprite;
            }
        }
    }

    #endregion

    #region Equipment
    public void OpenEquipment()
    {
        gameMenuUI.submenuNameText.text = "Gear";
        gameMenuUI.inventoryUIGO.SetActive(false);
        gameMenuUI.gearUIGO.SetActive(true);

        currentSpellButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.spellsInventory[0];
        currentSpellButton.GetComponent<Image>().sprite =
            playerInventory.spellsInventory[0].dataIcon;

        currentWeaponButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.weaponsInventory[0];
        currentWeaponButton.GetComponent<Image>().sprite =
            playerInventory.weaponsInventory[0].dataIcon;

        equipButton.SetActive(true);
    }

    public void SelectEquipmentToChange(DataSlotUI dataSlotUI)
    {
        DataObject itemToDisplay = dataSlotUI.slotData;

        if (itemToDisplay != null)
        {
            equipmentGrid.SetActive(true);
            gearSelectionOverviewButton.SetActive(true);
            gearSelectionOverview.SetActive(false);

            gameMenuUI.infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
            gameMenuUI.infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
            gameMenuUI.infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;
            gameMenuUI.infoPanel.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if(dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[0];

                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    Image myItemIcon = equipmentSlots[i].GetComponent<Image>();
                    DataSlotUI itemSlotUI = equipmentSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.weaponsInventory.Count)
                    {
                        itemSlotUI.slotData = playerInventory.weaponsInventory[i];

                        myItemIcon.sprite = playerInventory.weaponsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemSlotUI.slotData = null;
                        myItemIcon.sprite = emptyWindowSprite;
                    }
                }
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[0];

                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    Image myItemIcon = equipmentSlots[i].GetComponent<Image>();
                    DataSlotUI itemSlotUI = equipmentSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.spellsInventory.Count)
                    {
                        itemSlotUI.slotData = playerInventory.spellsInventory[i];

                        myItemIcon.sprite = playerInventory.spellsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemSlotUI.slotData = null;
                        myItemIcon.sprite = emptyWindowSprite;
                    }
                }
            }


        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.errorUIButtonSFX);
        }
    }


    public void SelectEquipmentOverview()
    {
        equipmentGrid.SetActive(false);
        gearSelectionOverview.SetActive(true);
        gameMenuUI.infoPanel.SetActive(false);
    }

    public void SelectEquipmentToView(DataSlotUI dataSlotUI)
    {
        if(dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            equipButton.GetComponent<DataSlotUI>().slotData = (MeleeWeapon)dataSlotUI.slotData;
        }
        else if (dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            equipButton.GetComponent<DataSlotUI>().slotData = (Spell)dataSlotUI.slotData;
        }
    }


    public void ChangeEquipment(DataSlotUI dataSlotUI)
    {
        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            if(playerInventory.activeWeapon == dataSlotUI.slotData)
            {
                return;
            }

            playerInventory.activeWeapon = (MeleeWeapon)dataSlotUI.slotData;
            playerMeleeHandler.DestroyMelee();
            playerMeleeHandler.SetParentOverride();
            playerMeleeHandler.LoadMelee();
        }
        else if (dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            if (playerInventory.spellsInventory[0] == dataSlotUI.slotData)
            {
                return;
            }

            playerInventory.activeSpell = (Spell)dataSlotUI.slotData;

        }

    }
    #endregion
}