using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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
    public GameObject equipButton;
    public GameObject cycleEquipmentButton;
    public GameObject currentSpellButton;
    public GameObject currentWeaponButton;
    public int currentEquipmentIndex = 0;


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
        cycleEquipmentButton.SetActive(true);
    }

    public void SelectEquipmentToChange(DataSlotUI dataSlotUI)
    {
        DataObject itemToDisplay = dataSlotUI.slotData;
        currentEquipmentIndex = 0;

        if (itemToDisplay != null)
        {
            gameMenuUI.infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
            gameMenuUI.infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
            gameMenuUI.infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;
            gameMenuUI.infoPanel.SetActive(true);
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

            if(dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
            {
                Debug.Log("You're trying to switch your weapon");
                equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[currentEquipmentIndex];
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[currentEquipmentIndex + 1];
            }
            else if(dataSlotUI.slotData.GetType() == typeof(Spell))
            {
                Debug.Log("You're trying to switch your spell");
                equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[currentEquipmentIndex];
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[currentEquipmentIndex + 1];
            }
        }
        else
        {
            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.errorUIButtonSFX);
        }
    }



    public void CycleEquipment(DataSlotUI dataSlotUI)
    {
        currentEquipmentIndex++;

        DataObject itemToDisplay = dataSlotUI.slotData;

        gameMenuUI.infoPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataName;
        gameMenuUI.infoPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemToDisplay.dataDescription;
        gameMenuUI.infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemToDisplay.dataIcon;
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[currentEquipmentIndex];

            if (currentEquipmentIndex == playerInventory.weaponsInventory.Count - 1)
            {
                currentEquipmentIndex = 0;
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[currentEquipmentIndex];
            }
            else
            {
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.weaponsInventory[currentEquipmentIndex + 1];
            }

        }
        else if (dataSlotUI.slotData.GetType() == typeof(Spell))
        {
            equipButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[currentEquipmentIndex];
            Debug.Log("Current weapon index: " + currentEquipmentIndex);
            Debug.Log("Spell count: " + playerInventory.spellsInventory.Count);

            if (currentEquipmentIndex == playerInventory.spellsInventory.Count - 1)
            {
                Debug.Log("Last item");
                currentEquipmentIndex = 0;
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[currentEquipmentIndex];
            }
            else
            {
                cycleEquipmentButton.GetComponent<DataSlotUI>().slotData = playerInventory.spellsInventory[currentEquipmentIndex + 1];
            }

        }
    }

    public void ChangeEquipment(DataSlotUI dataSlotUI)
    {
        if (dataSlotUI.slotData.GetType() == typeof(MeleeWeapon))
        {
            if(playerInventory.weaponsInventory[0] == dataSlotUI.slotData)
            {
                return;
            }

            playerInventory.weaponsInventory.Add(playerInventory.weaponsInventory[0]);
            playerInventory.weaponsInventory[0] = (MeleeWeapon)dataSlotUI.slotData;
            playerInventory.weaponsInventory.Remove(playerInventory.weaponsInventory[currentEquipmentIndex]);

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

            playerInventory.spellsInventory.Add(playerInventory.spellsInventory[0]);
            playerInventory.spellsInventory[0] = (Spell)dataSlotUI.slotData;
            playerInventory.spellsInventory.Remove(playerInventory.spellsInventory[currentEquipmentIndex]);
        }
    }
    #endregion
}