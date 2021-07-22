using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool leadsToNextRoom;
    public bool leadsToPreviousRoom;
    public Room otherRoom;
    public int doorNum = 0;
    [Tooltip("If there is no lock, leave as null.")]
    public string lockString;
    bool doorIsLocked = false;
    public GameObject lockedDoorMessageGO;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        TransitionScenes(playerManager, playerStats, playerStats.GetComponent<PlayerInventory>());
    }

    private void TransitionScenes(PlayerManager playerManager, PlayerStats playerStats, PlayerInventory playerInventory)
    {
        CheckForLock(playerInventory);

        if(!doorIsLocked)
        {
            SceneChangeManager sceneChangeManager = playerManager.GetComponentInParent<SceneChangeManager>();

            if (leadsToNextRoom)
            {
                playerManager.nextDoorNum = doorNum;

                StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex + 1].sceneNum));
            }
            else if (leadsToPreviousRoom)
            {
                playerManager.nextDoorNum = doorNum;

                StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex - 1].sceneNum));
            }
            else
            {
                if (otherRoom != null)
                {
                    playerManager.nextDoorNum = doorNum;

                    StartCoroutine(SceneChangeManager.Instance.ChangeScene(otherRoom.sceneNum));
                }
            }

            playerStats.EnableInvulnerability(1.5f);
        }
        else
        {
            DisplayLockedDoorFX();
        }

    }

    private void CheckForLock(PlayerInventory playerInventory)
    {
        if(lockString != null)
        {
            doorIsLocked = true;

            for (int i = 0; i < playerInventory.itemInventory.Count; i++)
            {
                if(playerInventory.itemInventory[i].name == lockString)
                {
                    doorIsLocked = false;
                }
            }
        }
    }

    private void DisplayLockedDoorFX()
    {
        Instantiate(lockedDoorMessageGO, transform.position, Quaternion.identity);
    }
}
