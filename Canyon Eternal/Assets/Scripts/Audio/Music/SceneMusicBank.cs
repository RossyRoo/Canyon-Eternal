using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Banks / Scene Music")]
public class SceneMusicBank : ScriptableObject
{
    public AudioClip autoMusic;
    public bool isInterruptingTrack = true;
}
