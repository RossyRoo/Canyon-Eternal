using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Data")]
public class Room : ScriptableObject
{
    [Header("INFO")]
    public string roomName;
    [TextArea]public string roomOverview;
    public bool isCheckpoint;
    [HideInInspector]public int sceneNum;


    [Header("AUDIO")]
    public AudioClip autoMusic;
    public bool isInterruptingTrack = true;

}
