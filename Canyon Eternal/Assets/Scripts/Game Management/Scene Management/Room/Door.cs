using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public int sceneNum;
    public int doorNum = 0;

    public RoomData nextRoom;
    public Vector3 nextRoomSpawnPoint;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        TransitionScenes(playerManager);
    }

    private void TransitionScenes(PlayerManager playerManager)
    {
        nextRoomSpawnPoint = nextRoom.spawnPoints[doorNum];

        playerManager.nextSpawnPoint = nextRoomSpawnPoint;

        StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneNum));
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            nextRoomSpawnPoint = nextRoom.spawnPoints[doorNum];

            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            playerManager.nextSpawnPoint = nextRoomSpawnPoint;

            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneNum));
        }
    }*/
}
