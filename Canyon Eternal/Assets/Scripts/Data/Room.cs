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

    [Tooltip("1 = Calm. 2 = Sunny. 3 = Breezy. 4 = Rainy. 5 = Acid Rain. 6 = Foggy. 7 = Snowy.")]
    public List <int> possibleWeatherPatterns;
}
