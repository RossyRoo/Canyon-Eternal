using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellphoneUI : MonoBehaviour
{
    PlayerInventory playerInventory;
    PlayerProgression playerProgression;

    public GameMenuUI gameMenuUI;
    public GameObject cellUIGO;
    public TextMeshProUGUI submenuNameText;
    public GameObject contactsUIGO;
    public GameObject photosUIGO;
    public GameObject settingsUIGO;

    [Header("Photos")]
    public GameObject photosInfoPanel;
    public Image photoIcon;
    public TextMeshProUGUI photoCaption;
    public GameObject[] photoSlots;

    [Header("Contacts")]
    public GameObject contactsInfoPanel;
    Contact activeContact;
    public Image contactIcon;
    public TextMeshProUGUI contactName;
    public TextMeshProUGUI contactBio;
    public GameObject[] contactSlots;

    [Header("Settings")]
    public GameObject settingsInfoPanel;


    public void OpenCellphone(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Cell";
        cellUIGO.SetActive(true);

        if (currentSubmenuIndex == 0)
        {
            OpenContacts();
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

    #region Contacts

    public void OpenContacts()
    {
        playerProgression = FindObjectOfType<PlayerProgression>();

        submenuNameText.text = "Messages";
        photosUIGO.SetActive(false);
        settingsUIGO.SetActive(false);
        contactsUIGO.SetActive(true);

        for (int i = 0; i < contactSlots.Length; i++)
        {
            Image myContactIcon = contactSlots[i].GetComponent<Image>();
            ContactSlotUI contactSlotUI = contactSlots[i].GetComponent<ContactSlotUI>();

            if (i < playerProgression.collectedContacts.Count)
            {
                contactSlotUI.slotContact = playerProgression.collectedContacts[i];
                myContactIcon.sprite = playerProgression.collectedContacts[i].itemIcon;

                myContactIcon.gameObject.SetActive(true);
            }
            else
            {
                myContactIcon.sprite = null;
                myContactIcon.gameObject.SetActive(false);
            }

        }
    }

    public void SelectDisplayContact(ContactSlotUI contactToSelect)
    {
        Contact contactToDisplay = contactToSelect.slotContact;

        if(contactToDisplay != null)
        {
            contactsInfoPanel.SetActive(true);
            activeContact = contactToDisplay;
            contactIcon.sprite = contactToDisplay.itemIcon;
            contactBio.text = contactToDisplay.itemDescription;
            contactName.text = contactToDisplay.itemName;
        }
    }

    public void TriggerOutgoingPhoneCall()
    {
        List<GameObject> possiblePhoneCalls = new List<GameObject>();

        for (int i = 0; i < activeContact.outgoingPhoneCalls.Count; i++)
        {
            if (playerProgression.playerVesselPercentage >= activeContact.outgoingPhoneCalls[i].GetComponent<PhoneCall>().minVesselPercentage
                && playerProgression.playerVesselPercentage <= activeContact.outgoingPhoneCalls[i].GetComponent<PhoneCall>().maxVesselPercentage
                && playerProgression.enemiesEncountered.Contains(activeContact.outgoingPhoneCalls[i].GetComponent<PhoneCall>().enemyEncounteredRequirement)
                || activeContact.outgoingPhoneCalls[i].GetComponent<PhoneCall>().enemyEncounteredRequirement == null
                && !playerProgression.collectedPhoneCallIDs.Contains(activeContact.outgoingPhoneCalls[i].GetComponent<PhoneCall>().phoneCallID))
            {
                possiblePhoneCalls.Add(activeContact.outgoingPhoneCalls[i]);
            }
        }

        int randomPhoneCallNum = Random.Range(0, possiblePhoneCalls.Count);

        playerProgression.GetComponentInParent<PlayerManager>().EnterConversationState();
        Instantiate(possiblePhoneCalls[randomPhoneCallNum], transform.position, Quaternion.identity);

        gameMenuUI.ReverseGameMenuUI();
    }
#endregion

    #region Photos

    public void OpenPhotos()
    {
        submenuNameText.text = "Photos";
        contactsUIGO.SetActive(false);
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


    public void SelectDisplayPhoto(ItemSlotUI slotToSelect)
    {
        Item photoToDisplay = slotToSelect.slotItem;

        if(photoToDisplay!=null)
        {
            photosInfoPanel.SetActive(true);
            photoIcon.sprite = photoToDisplay.itemIcon;
            photoCaption.text = photoToDisplay.itemDescription;
        }
    }

    #endregion

    #region Settings
    public void OpenSettings()
    {
        submenuNameText.text = "Settings";
        contactsUIGO.SetActive(false);
        photosUIGO.SetActive(false);
        settingsUIGO.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        GetComponentInParent<SceneChangeManager>().LoadMainMenu();
    }

    public void SelectSettingsToDisplay()
    {
        settingsInfoPanel.SetActive(true);
    }
    #endregion

}
