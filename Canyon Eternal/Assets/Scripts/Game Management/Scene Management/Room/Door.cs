using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool leadsToNextRoom;
    public bool leadsToPreviousRoom;
    public Room otherRoom;
    public int doorNum = 0;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        TransitionScenes(playerManager, playerStats);
    }

    private void TransitionScenes(PlayerManager playerManager, PlayerStats playerStats)
    {
        SceneChangeManager sceneChangeManager = playerManager.GetComponentInParent<SceneChangeManager>();

        if (leadsToNextRoom)
        {
            playerManager.nextDoorNum = doorNum;

            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex + 1].sceneNum));
        }
        else if(leadsToPreviousRoom)
        {
            playerManager.nextDoorNum = doorNum;

            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex - 1].sceneNum));
        }
        else
        {
            if(otherRoom != null)
            {
                playerManager.nextDoorNum = doorNum;

                StartCoroutine(SceneChangeManager.Instance.ChangeScene(otherRoom.sceneNum));
            }
        }


        playerStats.EnableInvulnerability(1.5f);

    }
}
