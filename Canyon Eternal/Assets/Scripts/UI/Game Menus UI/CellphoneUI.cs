using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellphoneUI : MonoBehaviour
{
    public GameMenuUI gameMenuUI;
    public GameObject cellUIGO;
    public TextMeshProUGUI submenuNameText;
    public GameObject messagesUIGO;
    public GameObject photosUIGO;
    public GameObject settingsUIGO;

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
    }

    public void OpenSettings()
    {
        submenuNameText.text = "Settings";
        messagesUIGO.SetActive(false);
        photosUIGO.SetActive(false);
        settingsUIGO.SetActive(true);


    }
}
