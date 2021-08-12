using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Data")]
public class Room : DataObject
{
    [Header("INFO")]
    public bool isCheckpoint;
    public bool isFastTravelPoint;
    public int sceneNum;
    public AudioClip roomAudio;
}
