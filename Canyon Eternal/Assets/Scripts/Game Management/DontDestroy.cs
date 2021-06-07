using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    PlayerManager playerManager;
    SFXPlayer sFXPlayer;
    MusicPlayer musicPlayer;
    CinemachineManager cinemachineManager;
    SceneChangeManager sceneChangeManager;
    ScreenFader screenFader;

    bool isPersistent;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        HandleDuplicates();
    }

    private void HandleDuplicates()
    {
        DontDestroy[] dontDestroyDuplicates = FindObjectsOfType<DontDestroy>();

        int duplicateCount = 0;

        foreach (var dontDestroyDuplicate in dontDestroyDuplicates)
        {
            duplicateCount += 1;

            playerManager = dontDestroyDuplicate.GetComponentInChildren<PlayerManager>();
            sFXPlayer = dontDestroyDuplicate.GetComponentInChildren<SFXPlayer>();
            musicPlayer = dontDestroyDuplicate.GetComponentInChildren<MusicPlayer>();
            sceneChangeManager = dontDestroyDuplicate.GetComponentInChildren<SceneChangeManager>();
            cinemachineManager = dontDestroyDuplicate.GetComponentInChildren<CinemachineManager>();
            screenFader = dontDestroyDuplicate.GetComponentInChildren<ScreenFader>();

            sceneChangeManager.FindCurrentRoom(dontDestroyDuplicate.sceneChangeManager);

            HandleOnLoadSceneFunctions();
        }

        if (duplicateCount == 1)
        {
            isPersistent = true;
        }

        else
        {
            if(!isPersistent)
            {
                Destroy(this.gameObject);
            }
        }

    }

    private void HandleOnLoadSceneFunctions()
    {
        sceneChangeManager.OnLoadScene();
        playerManager.OnLoadScene(sceneChangeManager.currentRoom);
        cinemachineManager.OnLoadScene(playerManager);
        musicPlayer.OnLoadScene(sceneChangeManager.currentRoom);
        sFXPlayer.OnLoadScene();
        screenFader.OnLoadScene();
    }
}
