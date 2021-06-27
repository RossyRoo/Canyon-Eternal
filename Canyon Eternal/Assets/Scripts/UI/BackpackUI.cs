using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//"Q" and "E" cycle menus left and right
//"A" and "D" cycle submenus while "W" and "S" cycle items

public class BackpackUI : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    PlayerInventory playerInventory;

    public GameObject backpackUIGO;
    public TextMeshProUGUI menuNameText;
    public TextMeshProUGUI submenuNameText;

    public GameObject[] interfaceSlots;
    public GameObject[] interfacePages;
    public Item viewingItem;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    [HideInInspector]
    public bool backpackIsOpen;

    [Header("Navigation")]
    public int interfacePageIndex = 0;
    public int currentMenuIndex = 0;
    public int currentSubmenuIndex = 0;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        playerInventory = playerAnimatorHandler.GetComponentInParent<PlayerInventory>();

        FindSubmenu();
    }

    public void ReverseBackpackUI()
    {
        if(backpackUIGO.activeInHierarchy)
        {
            backpackIsOpen = false;
            playerManager.isInteractingWithUI = false;
            playerAnimatorHandler.animator.SetBool("isInteracting", false);
            backpackUIGO.SetActive(false);
        }
        else
        {
            //DisplayNewIconAndDescription();
            backpackIsOpen = true;
            playerManager.isInteractingWithUI = true;
            playerAnimatorHandler.animator.SetBool("isInteracting", true);
            backpackUIGO.SetActive(true);
        }
    }

    private void DisplayNewIconAndDescription(Item item)
    {
        itemIcon.sprite = item.itemIcon;
        itemDescriptionText.text = item.itemDescription;
    }

    #region Cycle Menus

    public void CycleMenuRight()
    {
        if(currentMenuIndex == 0)
        {
            currentMenuIndex++;
        }
        else if(currentMenuIndex == 1)
        {
            currentMenuIndex++;
        }
        else
        {
            currentMenuIndex = 0;
        }

        FindSubmenu();
    }

    public void CycleMenuLeft()
    {
        if (currentMenuIndex == 0)
        {
            currentMenuIndex = 2;
        }
        else if (currentMenuIndex == 1)
        {
            currentMenuIndex--;
        }
        else
        {
            currentMenuIndex--;
        }

        FindSubmenu();
    }

    #endregion

    #region Cycle Submenus

    public void CycleSubmenuRight()
    {
        if (currentSubmenuIndex == 0)
        {
            currentSubmenuIndex++;
        }
        else if (currentSubmenuIndex == 1)
        {
            currentSubmenuIndex++;
        }
        else
        {
            currentSubmenuIndex = 0;
        }

        FindSubmenu();
    }

    public void CycleSubmenuLeft()
    {
        if (currentSubmenuIndex == 0)
        {
            currentSubmenuIndex = 2;
        }
        else if (currentMenuIndex == 1)
        {
            currentSubmenuIndex--;
        }
        else
        {
            currentSubmenuIndex--;
        }

        FindSubmenu();
    }

    private void FindSubmenu()
    {
        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            RawImage itemIcon = interfaceSlots[i].GetComponentInChildren<RawImage>();

            if(itemIcon != null)
            {
                Debug.Log("Raw Image found");
            }
            /*if(itemIcon.gameObject.activeInHierarchy)
            {
                itemIcon.gameObject.SetActive(false);
            }*/
        }

        if (currentMenuIndex == 0)
        {
            menuNameText.text = "Scrapbook";

            if (currentSubmenuIndex == 0)
            {
                OpenMap();
            }
            else if(currentSubmenuIndex == 1)
            {
                OpenJournal();
            }
            else
            {
                OpenBeastiary();
            }
        }
        else if(currentMenuIndex == 1)
        {
            menuNameText.text = "Backpack";

            if (currentSubmenuIndex == 0)
            {
                OpenKeyInventory();
            }
            else if (currentSubmenuIndex == 1)
            {
                OpenUsableInventory();
            }
            else
            {
                OpenTreasureInventory();
            }
        }
        else
        {
            menuNameText.text = "Cellphone";

            if (currentSubmenuIndex == 0)
            {
                OpenMessages();
            }
            else if (currentSubmenuIndex == 1)
            {
                OpenPhotos();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    #endregion

    public void CycleInterfacePages()
    {
        if(interfacePageIndex < interfacePages.Length)
        {
            interfacePageIndex++;
        }
        else
        {
            interfacePageIndex = 0;
        }

        for (int i = 0; i < interfacePages.Length; i++)
        {
            if(i == interfacePageIndex)
            {
                interfacePages[i].SetActive(true);
            }
            else
            {
                interfacePages[i].SetActive(false);
            }
        }
    }

    #region Submenus

    private void OpenMap()
    {
        submenuNameText.text = "Map";

    }

    private void OpenJournal()
    {
        submenuNameText.text = "Journal";

    }

    private void OpenBeastiary()
    {
        submenuNameText.text = "Beastiary";

    }

    private void OpenUsableInventory()
    {
        submenuNameText.text = "Usables";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            if(i < playerInventory.usableInventory.Count)
            {
                RawImage itemIcon = interfaceSlots[i].GetComponentInChildren<RawImage>();
                itemIcon.texture = playerInventory.usableInventory[i].itemIcon.texture;
                itemIcon.gameObject.SetActive(true);
            }
        }
    }

    private void OpenKeyInventory()
    {
        submenuNameText.text = "Keys";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            if (i < playerInventory.keyInventory.Count)
            {
                RawImage itemIcon = interfaceSlots[i].GetComponentInChildren<RawImage>();
                itemIcon.texture = playerInventory.keyInventory[i].itemIcon.texture;
                itemIcon.gameObject.SetActive(true);
            }
        }
    }

    private void OpenTreasureInventory()
    {
        submenuNameText.text = "Treasures";

        for (int i = 0; i < interfaceSlots.Length; i++)
        {
            if (i < playerInventory.treasureInventory.Count)
            {
                RawImage itemIcon = interfaceSlots[i].GetComponentInChildren<RawImage>();
                itemIcon.texture = playerInventory.treasureInventory[i].itemIcon.texture;
                itemIcon.gameObject.SetActive(true);
            }
        }
    }

    private void OpenMessages()
    {
        submenuNameText.text = "Messages";
    }

    private void OpenPhotos()
    {
        submenuNameText.text = "Photos";
    }


    private void OpenSettings()
    {
        submenuNameText.text = "Settings";
    }

    #endregion

    public void CycleItem()
    {
        //change the icon to this item's icon
        //change the description text to this item's description text
    }

}
