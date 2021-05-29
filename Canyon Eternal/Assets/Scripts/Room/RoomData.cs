using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    [Header("SPAWN POINTS")]
    public Vector3[] spawnPoints;

}
