using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Data")]
public class Room : ScriptableObject
{
    public string roomName;
    [TextArea]public string roomOverview;

    public int sceneNum;

    [Header("SPAWN POINTS")]
    public Vector3[] spawnPoints;

    [Header("AUDIO")]
    public AudioClip autoMusic;
    public bool isInterruptingTrack = true;

    public bool isCheckpoint;
}
