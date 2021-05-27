using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    bool isPersistent;

    PlayerManager playerManager;
    SFXPlayer sFXPlayer;
    CinemachineShake cinemachineShake;
    SceneChangeManager sceneChangeManager;

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
            sceneChangeManager = dontDestroyDuplicate.GetComponentInChildren<SceneChangeManager>();
            cinemachineShake = dontDestroyDuplicate.GetComponentInChildren<CinemachineShake>();

            HandleOnLoadSceneFunctions();
        }

        if(duplicateCount == 1)
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
        playerManager.OnLoadScene();
        cinemachineShake.OnLoadScene();
        sceneChangeManager.OnLoadScene();
    }
}
