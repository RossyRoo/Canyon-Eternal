using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    bool isPersistent;

    PlayerManager playerManager;
    SFXPlayer sFXPlayer;
    MusicPlayer musicPlayer;
    CinemachineShake cinemachineShake;
    SceneChangeManager sceneChangeManager;

    public RoomData currentRoom;

    private void Awake()
    {
        currentRoom = FindObjectOfType<RoomDataHolder>().thisRoom;
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
            cinemachineShake = dontDestroyDuplicate.GetComponentInChildren<CinemachineShake>();

            if(dontDestroyDuplicate.currentRoom == null) //destroyed GO passes off its room data to the persistant game objects
            {
                dontDestroyDuplicate.currentRoom = currentRoom;
            }

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
        sFXPlayer.OnLoadScene();
        playerManager.OnLoadScene(currentRoom);
        cinemachineShake.OnLoadScene();
        sceneChangeManager.OnLoadScene();
        musicPlayer.OnLoadScene(currentRoom);
    }
}
