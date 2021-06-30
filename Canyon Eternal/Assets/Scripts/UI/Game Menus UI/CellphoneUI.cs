using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellphoneUI : MonoBehaviour
{
    PlayerInventory playerInventory;

    public GameMenuUI gameMenuUI;
    public GameObject cellUIGO;
    public TextMeshProUGUI submenuNameText;
    public GameObject messagesUIGO;
    public GameObject photosUIGO;
    public GameObject settingsUIGO;

    [Header("Photos")]
    public Image photoIcon;
    public TextMeshProUGUI photoCaption;
    public GameObject[] photoSlots;


    public void OpenCellphone(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Cell";
        cellUIGO.SetActive(true);

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

    public void CloseCellphone()
    {
        cellUIGO.SetActive(false);
    }

    public void OpenMessages()
    {
        submenuNameText.text = "Messages";
        photosUIGO.SetActive(false);
        settingsUIGO.SetActive(false);
        messagesUIGO.SetActive(true);
    }

    public void OpenPhotos()
    {
        submenuNameText.text = "Photos";
        messagesUIGO.SetActive(false);
        settingsUIGO.SetActive(false);
        photosUIGO.SetActive(true);


        for (int i = 0; i < photoSlots.Length; i++)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            PlayerProgression playerProgression = FindObjectOfType<PlayerProgression>();
            ItemSlotUI itemSlotUI = photoSlots[i].GetComponent<ItemSlotUI>();

            if (i < playerInventory.photoInventory.Count)
            {
                FindPhotoSlotIcons(i, playerInventory, playerProgression);
                itemSlotUI.slotItem = playerInventory.photoInventory[i];
                photoSlots[i].SetActive(true);
            }
            else
            {
                photoSlots[i].SetActive(false);
            }
        }

        DisplayPhoto(playerInventory.photoInventory[0]);
    }

    private void FindPhotoSlotIcons(int i, PlayerInventory playerInventory, PlayerProgression playerProgression)
    {
        playerProgression.AdjustVesselLevel(0);
        Photo thisPhoto = playerInventory.photoInventory[i];

        if (playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent2)
        {
            photoSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon4;
            thisPhoto.itemIcon = playerInventory.photoInventory[i].photoIcon4;
            thisPhoto.itemDescription = playerInventory.photoInventory[i].photoCaption4;
        }
        else if(playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent2)
        {
            photoSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon3;
            thisPhoto.itemIcon = playerInventory.photoInventory[i].photoIcon3;
            thisPhoto.itemDescription = playerInventory.photoInventory[i].photoCaption3;
        }
        else if (playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent1)
        {
            photoSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon2;
            thisPhoto.itemIcon = playerInventory.photoInventory[i].photoIcon2;
            thisPhoto.itemDescription = playerInventory.photoInventory[i].photoCaption2;
        }
        else
        {
            photoSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon1;
            thisPhoto.itemIcon = playerInventory.photoInventory[i].photoIcon1;
            thisPhoto.itemDescription = playerInventory.photoInventory[i].photoCaption1;
        }
    }

    public void OpenSettings()
    {
        submenuNameText.text = "Settings";
        messagesUIGO.SetActive(false);
        photosUIGO.SetActive(false);
        settingsUIGO.SetActive(true);
    }

    private void DisplayPhoto(Item itemToDisplay)
    {
        photoIcon.sprite = itemToDisplay.itemIcon;
        photoCaption.text = itemToDisplay.itemDescription;
    }

    public void SelectDisplayItem(ItemSlotUI slotToSelect)
    {
        DisplayPhoto(slotToSelect.slotItem);
    }
}
