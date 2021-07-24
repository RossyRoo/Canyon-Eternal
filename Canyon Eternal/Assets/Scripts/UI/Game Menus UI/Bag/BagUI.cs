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

    public GameObject currentSpellButton;
    public GameObject currentWeaponButton;
    public GameObject currentGearButton;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMeleeHandler = FindObjectOfType<PlayerMeleeHandler>();

        gameMenuUI.menuNameText.text = "Bag";

        for (int i = 0; i < gameMenuUI.interfacePages.Length; i++)
        {
            if (i == 0)
            {
                gameMenuUI.interfacePages[i].SetActive(true);
            }
            else
            {
                gameMenuUI.interfacePages[i].SetActive(false);
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
        SelectEquipmentOverview();
        gameMenuUI.equipButton.SetActive(false);
        gameMenuUI.equipmentOverviewButton.SetActive(false);
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.equipmentUIGO.SetActive(false);
    }

    #region Inventory

    public void OpenItemInventory()
    {
        gameMenuUI.submenuNameText.text = "Inventory";
        gameMenuUI.equipmentUIGO.SetActive(false);
        gameMenuUI.RefreshGrid(true);


        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.itemInventory.Count)
            {
                itemSlotUI.slotData = playerInventory.itemInventory[i];

                myItemIcon.sprite = playerInventory.itemInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Artifacts

    public void OpenArtifacts()
    {
        gameMenuUI.submenuNameText.text = "Artifacts";
        gameMenuUI.equipmentUIGO.SetActive(false);
        gameMenuUI.RefreshGrid(true);

        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.artifactInventory.Count)
            {
                itemSlotUI.slotData = playerInventory.artifactInventory[i];

                myItemIcon.sprite = playerInventory.artifactInventory[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemSlotUI.slotData = null;
                myItemIcon.sprite = gameMenuUI.emptyWindowSprite;
            }
        }
    }

    #endregion

    #region Equipment
    public void OpenEquipment()
    {
        gameMenuUI.submenuNameText.text = "Equipment";
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.equipmentUIGO.SetActive(true);

        currentSpellButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.spellsInventory[0];
        currentSpellButton.GetComponent<Image>().sprite =
            playerInventory.spellsInventory[0].dataIcon;

        currentWeaponButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.weaponsInventory[0];
        currentWeaponButton.GetComponent<Image>().sprite =
            playerInventory.weaponsInventory[0].dataIcon;

        currentGearButton.GetComponent<DataSlotUI>().slotData =
            playerInventory.gearInventory[0];
        currentGearButton.GetComponent<Image>().sprite =
            playerInventory.gearInventory[0].dataIcon;

        gameMenuUI.equipButton.SetActive(true);
    }

    public void SelectEquipmentToChange(DataSlotUI dataSlotUI)
    {
        DataObject itemToDisplay = dataSlotUI.slotData;

        if (itemToDisplay != null)
        {
            gameMenuUI.interfaceGrid.SetActive(true);
            gameMenuUI.equipmentOverviewButton.SetActive(true);
            gameMenuUI.equipmentUIGO.SetActive(false);

            gameMenuUI.infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
            gameMenuUI.infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
            gameMenuUI.infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;
            gameMenuUI.infoPanel.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if(dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[0];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.weaponsInventory.Count)
                    {
                        itemSlotUI.slotData = playerInventory.weaponsInventory[i];

                        myItemIcon.sprite = playerInventory.weaponsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[0];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.spellsInventory.Count)
                    {
                        itemSlotUI.slotData = playerInventory.spellsInventory[i];

                        myItemIcon.sprite = playerInventory.spellsInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
                    }
                }
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Gear))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.gearInventory[0];

                for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
                {
                    Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
                    DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

                    if (i < playerInventory.gearInventory.Count)
                    {
                        itemSlotUI.slotData = playerInventory.gearInventory[i];

                        myItemIcon.sprite = playerInventory.gearInventory[i].dataIcon;
                        myItemIcon.gameObject.SetActive(true);
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
        gameMenuUI.interfaceGrid.SetActive(false);
        gameMenuUI.equipmentUIGO.SetActive(true);
        gameMenuUI.infoPanel.SetActive(false);
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);
    }

    public void SelectEquipmentToView(DataSlotUI dataSlotUI)
    {
        if(dataSlotUI.slotData != null)
        {
            if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (MeleeWeapon)dataSlotUI.slotData;
            }
            else if (dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (Spell)dataSlotUI.slotData;
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Gear))
            {
                gameMenuUI.equipButton.GetComponent<DataSlotUI>().slotData = (Gear)dataSlotUI.slotData;
            }
        }
    }

    public void ChangeEquipment(DataSlotUI dataSlotUI)
    {
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

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
            if (playerInventory.activeSpell == dataSlotUI.slotData)
            {
                return;
            }

            playerInventory.activeSpell = (Spell)dataSlotUI.slotData;
        }
        else if(dataSlotUI.slotData.GetType() == typeof(Gear))
        {
            if (playerInventory.activeGear == dataSlotUI.slotData)
            {
                return;
            }

            playerInventory.activeGear = (Gear)dataSlotUI.slotData;
            playerInventory.GetComponent<PlayerGearHandler>().InitializeGearEffect(playerInventory.activeGear.gearID);
        }

    }
    #endregion
}