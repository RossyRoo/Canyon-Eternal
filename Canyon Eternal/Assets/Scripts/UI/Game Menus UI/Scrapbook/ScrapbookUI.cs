using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrapbookUI : MonoBehaviour
{
    PlayerProgression playerProgression;
    SceneChangeManager sceneChangeManager;

    public GameMenuUI gameMenuUI;
    public TextMeshProUGUI submenuNameText;
    public GameObject scrapbookUIGO;
    public GameObject mapUIGO;
    public GameObject journalUIGO;
    public GameObject bestiaryUIGO;

    [Header("World Map")]
    public GameObject playerIcon;
    public GameObject worldMapUIGO;
    public GameObject[] worldMapAreas;

    [Header("Area Maps")]
    public GameObject areaMapUIGO;
    public GameObject[] areaMaps;
    public GameObject currentAreaMap;
    public AreaSlot[] roomsInCurrentArea;


    public void OpenScrapbook(int currentSubmenuIndex)
    {
        if(playerProgression == null || sceneChangeManager == null)
        {
            playerProgression = FindObjectOfType<PlayerProgression>();
            sceneChangeManager = FindObjectOfType<SceneChangeManager>();
        }

        gameMenuUI.menuNameText.text = null;
        scrapbookUIGO.SetActive(true);

        if(currentSubmenuIndex == 0)
        {
            OpenMap();
        }
        else if(currentSubmenuIndex == 1)
        {
            OpenJournal();
        }
        else
        {
            OpenBestiary();
        }
    }

    public void CloseScrapbook()
    {
        scrapbookUIGO.SetActive(false);
    }

    #region Map
    public void OpenMap()
    {
        submenuNameText.text = "";
        bestiaryUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        mapUIGO.SetActive(true);

        PrepareWorldMap();
    }


    public void PrepareWorldMap()
    {
        areaMapUIGO.SetActive(false);
        worldMapUIGO.SetActive(true);

        for (int i = 0; i < worldMapAreas.Length; i++)
        {
            AreaSlot thisArea = worldMapAreas[i].GetComponent<AreaSlot>();

            if(playerProgression.roomsDiscovered.Contains(thisArea.slotRoom))
            {
                if (thisArea.roomsInThisArea.Contains(sceneChangeManager.currentRoom))
                {
                    playerIcon.transform.position = thisArea.playerIconTF.position;
                }

                if (thisArea.areaCoverGO != null)
                {
                    thisArea.areaCoverGO.GetComponent<Animator>().Play("RemoveCover");

                    Destroy(thisArea.areaCoverGO, 1);
                }
            }
        }
    }

    public void PrepareAreaMap()
    {
        worldMapUIGO.SetActive(false);
        areaMapUIGO.SetActive(true);

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
                    thisRoom.areaCoverGO.GetComponent<Animator>().Play("RemoveCover");

                    Destroy(thisRoom.areaCoverGO, 1);
                }
            }
        }
    }

    public void SwitchToAreaMap(GameObject areaMap)
    {
        playerIcon.SetActive(false);

        areaMap.SetActive(true);
        currentAreaMap = areaMap;

        roomsInCurrentArea = currentAreaMap.GetComponentsInChildren<AreaSlot>();

        PrepareAreaMap();
    }

    public void SwitchToWorldMap()
    {
        playerIcon.SetActive(true);

        for (int i = 0; i < areaMaps.Length; i++)
        {
            areaMaps[i].SetActive(false);
        }
        PrepareWorldMap();
    }

    #endregion

    public void OpenJournal()
    {
        submenuNameText.text = "Journal";
        bestiaryUIGO.SetActive(false);
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(true);

    }

    public void OpenBestiary()
    {
        submenuNameText.text = "Bestiary";
        mapUIGO.SetActive(false);
        journalUIGO.SetActive(false);
        bestiaryUIGO.SetActive(true);

    }
}
