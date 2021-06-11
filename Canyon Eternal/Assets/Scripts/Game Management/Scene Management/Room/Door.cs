using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool leadsToNextRoom;
    public bool leadsToPreviousRoom;
    public Room otherRoom;

    public int doorNum = 0;
    Vector3 nextRoomSpawnPoint;


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
            Debug.Log("Going to next room # " + sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex + 1]);

            nextRoomSpawnPoint = sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex + 1].spawnPoints[doorNum];
            playerManager.nextSpawnPoint = nextRoomSpawnPoint;
            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex + 1].sceneNum));
        }
        else if(leadsToPreviousRoom)
        {
            nextRoomSpawnPoint = sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex - 1].spawnPoints[doorNum];
            playerManager.nextSpawnPoint = nextRoomSpawnPoint;
            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneChangeManager.roomList[sceneChangeManager.currentBuildIndex - 1].sceneNum));
            Debug.Log("Going to last room");
        }
        else
        {
            if(otherRoom != null)
            {
                nextRoomSpawnPoint = otherRoom.spawnPoints[doorNum];
                playerManager.nextSpawnPoint = nextRoomSpawnPoint;
                StartCoroutine(SceneChangeManager.Instance.ChangeScene(otherRoom.sceneNum));
            }
        }


        playerStats.EnableInvulnerability(1.5f);

    }
}
