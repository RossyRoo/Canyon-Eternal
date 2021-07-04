using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;
    public TextMeshProUGUI submenuNameText;
    PlayerInventory playerInventory;

    [Header("Interface Panel")]
    public GameObject bagUIGO;
    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public int interfacePageIndex = 0;

    [Header("Item Info Panel")]
    public GameObject bagInfoPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void OpenBag(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Bag";
        bagUIGO.SetActive(true);

        if (currentSubmenuIndex == 0)
        {
            OpenUsableInventory();
        }
        else if (currentSubmenuIndex == 1)
        {
            OpenKeyInventory();
        }
        else
        {
            OpenTreasureInventory();
        }
    }

    public void CloseBag()
    {
        bagUIGO.SetActive(false);
    }

    public void OpenUsableInventory()
    {
        submenuNameText.text = "Usables";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.usableInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.usableInventory[i];
                myItemIcon.sprite = playerInventory.usableInventory[i].itemIcon;

                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

    }

    public void OpenKeyInventory()
    {
        submenuNameText.text = "Keys";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.keyInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.keyInventory[i];

                myItemIcon.sprite = playerInventory.keyInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

    }

    public void OpenTreasureInventory()
    {
        submenuNameText.text = "Treasures";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();
            ItemSlotUI itemSlotUI = interfaceSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.treasureInventory.Count)
            {
                itemSlotUI.slotItem = playerInventory.treasureInventory[i];

                myItemIcon.sprite = playerInventory.treasureInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

    }

    private void DisplayItem(Item itemToDisplay)
    {
        itemIcon.sprite = itemToDisplay.itemIcon;
        itemDescriptionText.text = itemToDisplay.itemDescription;
        itemNameText.text = itemToDisplay.itemName;
    }

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
    }

    public void SelectDisplayItem(ItemSlotUI slotToSelect)
    {
        bagInfoPanel.SetActive(true);

        DisplayItem(slotToSelect.slotItem);
    }
}
