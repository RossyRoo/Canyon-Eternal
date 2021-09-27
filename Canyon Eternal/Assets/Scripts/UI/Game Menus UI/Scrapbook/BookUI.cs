using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookUI : MonoBehaviour
{
    PlayerProgression playerProgression;
    SceneChangeManager sceneChangeManager;
    GameMenuUI gameMenuUI;

    [Header("World Map")]
    public GameObject playerIcon;
    public GameObject worldMapUIGO;
    public GameObject[] worldMapAreas;

    [Header("Area Maps")]
    public GameObject areaMapUIGO;
    public GameObject[] areaMaps;
    GameObject currentAreaMap;
    AreaSlot[] roomsInCurrentArea;


    private void Awake()
    {
        gameMenuUI = GetComponent<GameMenuUI>();
    }

    public void OpenBook(int currentSubmenuIndex)
    {
        if (playerProgression == null)
        {
            playerProgression = FindObjectOfType<PlayerProgression>();
            sceneChangeManager = FindObjectOfType<SceneChangeManager>();
        }

        gameMenuUI.menuNameText.text = "Book";

        if(currentSubmenuIndex == 0)
        {
            OpenMap();
        }
        else if(currentSubmenuIndex == 1)
        {
            gameMenuUI.backButton.SetActive(false);
            OpenBestiary();
        }

    }

    public void CloseBook()
    {
        gameMenuUI.backButton.SetActive(false);
        gameMenuUI.mapUIGO.SetActive(false);
        gameMenuUI.RefreshGrid(false);
    }

    #region Map
    public void OpenMap()
    {
        gameMenuUI.submenuNameText.text = "Map";
        gameMenuUI.RefreshGrid(false);
        gameMenuUI.mapUIGO.SetActive(true);

        PrepareWorldMap();
    }


    public void PrepareWorldMap()
    {
        areaMapUIGO.SetActive(false);
        worldMapUIGO.SetActive(true);
        gameMenuUI.backButton.SetActive(false);

        for (int i = 0; i < worldMapAreas.Length; i++)
        {
            AreaSlot thisArea = worldMapAreas[i].GetComponent<AreaSlot>();

            for (int j = 0; j < thisArea.roomsInThisArea.Count; j++)
            {
                if(playerProgression.roomsDiscovered.Contains(thisArea.roomsInThisArea[j]))
                {
                    if (thisArea.roomsInThisArea.Contains(sceneChangeManager.currentRoom))
                    {
                        playerIcon.transform.position = thisArea.playerIconTF.position;
                    }

                    if (thisArea.areaCoverGO != null)
                    {
                        Animator[] coverAnimators = thisArea.areaCoverGO.GetComponentsInChildren<Animator>();
                        for (int k = 0; k < coverAnimators.Length; k++)
                        {
                            coverAnimators[k].Play("RemoveCover");
                        }

                        Destroy(thisArea.areaCoverGO, 1);
                    }
                }
            }
        }
    }

    public void PrepareAreaMap()
    {
        worldMapUIGO.SetActive(false);
        areaMapUIGO.SetActive(true);
        gameMenuUI.backButton.SetActive(true);

        for (int i = 0; i < roomsInCurrentArea.Length; i++)
        {
            AreaSlot thisRoom = roomsInCurrentArea[i].GetComponent<AreaSlot>();

            if (playerProgression.roomsDiscovered.Contains(thisRoom.slotRoom))
            {
                if (sceneChangeManager.currentRoom == thisRoom.slotRoom)
                {
                    playerIcon.SetActive(true);
                    playerIcon.transform.position = thisRoom.playerIconTF.position;
                }

                if (thisRoom.areaCoverGO != null)
                {
                    Animator[] coverAnimators = thisRoom.areaCoverGO.GetComponentsInChildren<Animator>();
                    for (int j = 0; j < coverAnimators.Length; j++)
                    {
                        coverAnimators[j].Play("RemoveCover");
                    }

                    Destroy(thisRoom.areaCoverGO, 1);
                }
            }
        }
    }

    public void SwitchToAreaMap(GameObject areaMap)
    {
        playerIcon.SetActive(false);

        areaMap.SetActive(true);
        gameMenuUI.backButton.SetActive(true);
        currentAreaMap = areaMap;

        roomsInCurrentArea = currentAreaMap.GetComponentsInChildren<AreaSlot>();

        PrepareAreaMap();

        SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);

    }

    public void SwitchToWorldMap()
    {
        if (gameMenuUI.currentMenuIndex == 0)
        {
            playerIcon.SetActive(true);

            for (int i = 0; i < areaMaps.Length; i++)
            {
                areaMaps[i].SetActive(false);
            }
            PrepareWorldMap();

            SFXPlayer.Instance.PlaySFXAudioClip(gameMenuUI.clickUIButtonSFX);
        }

    }

    #endregion

    #region Bestiary
    public void OpenBestiary()
    {
        gameMenuUI.submenuNameText.text = "Bestiary";
        gameMenuUI.RefreshGrid(true);
        gameMenuUI.mapUIGO.SetActive(false);

        for (int i = 0; i < gameMenuUI.interfaceGridSlots.Length; i++)
        {
            Image myEnemyIcon = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>().icon;
            DataSlotUI enemySlotUI = gameMenuUI.interfaceGridSlots[i].GetComponent<DataSlotUI>();

            if (i < playerProgression.enemiesEncountered.Count)
            {
                myEnemyIcon.gameObject.SetActive(true);
                enemySlotUI.slotData = playerProgression.enemiesEncountered[i];
                myEnemyIcon.sprite = playerProgression.enemiesEncountered[i].dataIcon;
            }
        }
    }

    #endregion


}
