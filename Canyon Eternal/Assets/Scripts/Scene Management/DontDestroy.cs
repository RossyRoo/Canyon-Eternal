using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
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

            PlayerManager playerManager = dontDestroyDuplicate.GetComponentInChildren<PlayerManager>();
            SFXPlayer sFXPlayer = dontDestroyDuplicate.GetComponentInChildren<SFXPlayer>();

            HandleNewSceneFunctions(playerManager, sFXPlayer);
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

    private void HandleNewSceneFunctions(PlayerManager playerManager, SFXPlayer sFXPlayer)
    {
        sFXPlayer.OnLoadScene();
        playerManager.OnLoadScene();
    }
}
