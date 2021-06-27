using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;
    PlayerInventory playerInventory;

    [Header("Interface Panel")]
    public GameObject interfacePanel;
    public GameObject itemInfoPanel;
    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public int interfacePageIndex = 0;

    [Header("Item Info Panel")]
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
        interfacePanel.SetActive(true);
        itemInfoPanel.SetActive(true);

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
        interfacePanel.SetActive(false);
        itemInfoPanel.SetActive(false);
    }

    public void OpenUsableInventory()
    {
        gameMenuUI.submenuNameText.text = "Usables";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();

            if (i < playerInventory.usableInventory.Count)
            {
                myItemIcon.sprite = playerInventory.usableInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

        DisplayItem(playerInventory.usableInventory[0]);
    }

    public void OpenKeyInventory()
    {
        gameMenuUI.submenuNameText.text = "Keys";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();

            if (i < playerInventory.keyInventory.Count)
            {
                myItemIcon.sprite = playerInventory.keyInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

        DisplayItem(playerInventory.keyInventory[0]);
    }

    public void OpenTreasureInventory()
    {
        gameMenuUI.submenuNameText.text = "Treasures";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            Image myItemIcon = interfaceSlots[i].GetComponent<Image>();

            if (i < playerInventory.treasureInventory.Count)
            {
                myItemIcon.sprite = playerInventory.treasureInventory[i].itemIcon;
                myItemIcon.gameObject.SetActive(true);
            }
            else
            {
                myItemIcon.sprite = null;
                myItemIcon.gameObject.SetActive(false);
            }

        }

        DisplayItem(playerInventory.treasureInventory[0]);
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
}
