using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelUI : MonoBehaviour
{
    GameMenuUI gameMenuUI;
    PlayerManager playerManager;
    BookUI bookUI;
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerProgression playerProgression;

    public bool fastTravelIsOpen;

    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
        bookUI = GetComponent<BookUI>();

    }

    public void OpenFastTravel()
    {
        //Find references
        playerManager = FindObjectOfType<PlayerManager>();
        playerAnimatorHandler = FindObjectOfType<PlayerAnimatorHandler>();
        playerProgression = FindObjectOfType<PlayerProgression>();

        //Set UI elements
        gameMenuUI.menuNameText.text = "Select";
        gameMenuUI.submenuNameText.text = "Location";
        bookUI.worldMapUIGO.SetActive(false);
        gameMenuUI.interfaceBackground.GetComponent<Image>().enabled = true;
        gameMenuUI.fastTravelButton.SetActive(true);

        //Put player in UI mode
        playerManager.isInteractingWithUI = true;
        playerAnimatorHandler.animator.SetBool("isInteracting", true);
        gameMenuUI.gameMenusGO.SetActive(true);
        fastTravelIsOpen = true;

        //Load Interface and Info panel
        gameMenuUI.RefreshGrid(true);

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


        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myItemIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<Image>();
            DataSlotUI itemSlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerProgression.fastTravelLocationsDiscovered.Count)
            {
                itemSlotUI.slotData = playerProgression.fastTravelLocationsDiscovered[i];

                myItemIcon.sprite = playerProgression.fastTravelLocationsDiscovered[i].dataIcon;
                myItemIcon.gameObject.SetActive(true);
            }
        }
        //INSERT SFX
    }

    public void CloseFastTravel()
    {
        if (fastTravelIsOpen)
        {
            fastTravelIsOpen = false;

            gameMenuUI.fastTravelButton.SetActive(false);
        }
    }

    public void InitiateFastTravelSequence(DataSlotUI fastTravelDestination)
    {

        Room selectedFastTravelLocation = (Room)fastTravelDestination.slotData;

        playerManager.nextDoorNum = 999;

        StartCoroutine(GetComponentInParent<SceneChangeManager>().ChangeScene(int.Parse(selectedFastTravelLocation.name)));
        gameMenuUI.gameMenusGO.SetActive(false);
        playerManager.isInteractingWithUI = false;
    }
}
