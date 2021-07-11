using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellphoneUI : MonoBehaviour
{
    PlayerInventory playerInventory;
    PlayerProgression playerProgression;
    GameMenuUI gameMenuUI;
    [HideInInspector]public Contact activeContact;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
    }

    public void OpenCellphone(int currentSubmenuIndex)
    {
        gameMenuUI.menuNameText.text = "Cell";

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

    public void CloseCellphone()
    {
        gameMenuUI.callButton.SetActive(false);
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.settingsUIGO.SetActive(false);
    }

    #region Contacts

    public void OpenContacts()
    {
        playerProgression = FindObjectOfType<PlayerProgression>();

        gameMenuUI.submenuNameText.text = "Contacts";
        gameMenuUI.settingsUIGO.SetActive(false);
        gameMenuUI.RefreshGrid(true);
        gameMenuUI.callButton.SetActive(true);

        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myContactIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerProgression.collectedContacts.Count)
            {
                itemSlotUI.slotData = playerProgression.collectedContacts[i];
                myContactIcon.sprite = playerProgression.collectedContacts[i].dataIcon;
            }
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
        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.phoneRingSFX);

        gameMenuUI.ReverseGameMenuUI();
    }
#endregion

    #region Photos

    public void OpenPhotos()
    {
        gameMenuUI.submenuNameText.text = "Photos";
        gameMenuUI.settingsUIGO.SetActive(false);
        gameMenuUI.RefreshGrid(true);


        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            PlayerProgression playerProgression = FindObjectOfType<PlayerProgression>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerInventory.photoInventory.Count)
            {
                FindPhotoSlotIcons(i, playerInventory, playerProgression);
                itemSlotUI.slotData = playerInventory.photoInventory[i];
                gameMenuUI.interfaceGridSlots[i].SetActive(true);
            }
        }
    }

    private void FindPhotoSlotIcons(int i, PlayerInventory playerInventory, PlayerProgression playerProgression)
    {
        playerProgression.AdjustVesselLevel(0);
        Photo thisPhoto = playerInventory.photoInventory[i];

        if (playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent2)
        {
            gameMenuUI.interfaceGridSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon4;
            thisPhoto.dataIcon = playerInventory.photoInventory[i].photoIcon4;
            thisPhoto.dataDescription = playerInventory.photoInventory[i].photoCaption4;
        }
        else if(playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent2)
        {
            gameMenuUI.interfaceGridSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon3;
            thisPhoto.dataIcon = playerInventory.photoInventory[i].photoIcon3;
            thisPhoto.dataDescription = playerInventory.photoInventory[i].photoCaption3;
        }
        else if (playerProgression.playerVesselPercentage >= playerInventory.photoInventory[i].turningPercent1)
        {
            gameMenuUI.interfaceGridSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon2;
            thisPhoto.dataIcon = playerInventory.photoInventory[i].photoIcon2;
            thisPhoto.dataDescription = playerInventory.photoInventory[i].photoCaption2;
        }
        else
        {
            gameMenuUI.interfaceGridSlots[i].GetComponent<Image>().sprite = playerInventory.photoInventory[i].photoIcon1;
            thisPhoto.dataIcon = playerInventory.photoInventory[i].photoIcon1;
            thisPhoto.dataDescription = playerInventory.photoInventory[i].photoCaption1;
        }
    }
    #endregion

    #region Settings
    public void OpenSettings()
    {
        gameMenuUI.submenuNameText.text = "Settings";
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.settingsUIGO.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        GetComponentInParent<SceneChangeManager>().LoadMainMenu();
    }
    #endregion

}
