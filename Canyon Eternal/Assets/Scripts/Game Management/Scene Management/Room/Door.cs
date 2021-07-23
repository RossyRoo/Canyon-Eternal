using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool leadsToNextRoom;
    public bool leadsToPreviousRoom;
    public Room otherRoom;
    public int doorNum = 0;

    [Header("Lock and Key")]
    [Tooltip("If there is no lock, leave as null.")]
    public Item requiredItem;
    bool doorIsLocked = false;
    public GameObject lockedDoorMessageGO;
    public AudioClip lockedSFX;

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
        if(requiredItem != null)
        {
            doorIsLocked = true;

            if (playerInventory.itemInventory.Contains(requiredItem))
            {
                doorIsLocked = false;
                return;
            }

        }
    }

    private void DisplayLockedDoorFX()
    {
        ObjectPool objectPool = FindObjectOfType<ObjectPool>();
        Instantiate(lockedDoorMessageGO, objectPool.transform.position, Quaternion.identity);
        SFXPlayer.Instance.PlaySFXAudioClip(lockedSFX);
    }
}
