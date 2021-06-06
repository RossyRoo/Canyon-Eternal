using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    bool isPersistent;

    public PlayerManager playerManager;
    SFXPlayer sFXPlayer;
    MusicPlayer musicPlayer;
    CinemachineManager cinemachineManager;
    SceneChangeManager sceneChangeManager;

    [Header("Scene")]
    public int currentBuildIndex;
    public Room currentRoom;
    public List<Room> allRoomsInOrder = new List<Room>();


    private void Awake()
    {
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentRoom = allRoomsInOrder[currentBuildIndex];
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

            dontDestroyDuplicate.currentRoom = currentRoom;


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
        cinemachineManager.OnLoadScene(playerManager);
        sceneChangeManager.OnLoadScene();
        musicPlayer.OnLoadScene(currentRoom);
    }
}
