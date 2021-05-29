using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int sceneNum;
    public int doorNum = 0;

    public RoomData nextRoom;
    public Vector3 nextRoomSpawnPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            nextRoomSpawnPoint = nextRoom.spawnPoints[doorNum];

            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            playerManager.nextSpawnPoint = nextRoomSpawnPoint;

            StartCoroutine(SceneChangeManager.Instance.ChangeScene(sceneNum));
        }
    }
}
