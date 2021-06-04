using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public int sceneNum;
    public int doorNum = 0;

    public RoomData nextRoom;
    public Vector3 nextRoomSpawnPoint;

    public override void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        base.Interact(playerManager, playerStats);
        TransitionScenes(playerManager, playerStats);
    }

    private void TransitionScenes(PlayerManager playerManager, PlayerStats playerStats)
    {
        nextRoomSpawnPoint = nextRoom.spawnPoints[doorNum];

        playerManager.nextSpawnPoint = nextRoomSpawnPoint;

        playerStats.EnableInvulnerability(1.5f);

        StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneNum));
    }
}
