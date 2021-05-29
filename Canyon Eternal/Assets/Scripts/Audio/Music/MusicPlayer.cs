using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    AudioSource audioSource;

    private void Start()
    {
        if(!GetComponentInParent<DontDestroy>())
        {
            RoomData currentRoom = FindObjectOfType<RoomData>();

            audioSource = GetComponent<AudioSource>();
            PlaySceneMusic(currentRoom);
        }
    }

    public void OnLoadScene(RoomData currentRoom)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        PlaySceneMusic(currentRoom);
    }

    private void PlaySceneMusic(RoomData currentRoom)
    {
        if (currentRoom.sceneMusicBank.autoMusic != null
            && currentRoom.sceneMusicBank.isInterruptingTrack
            || !audioSource.isPlaying)
        {
            //Check to see if this scene's music is supposed to override the other scene's

            audioSource.Stop();
            audioSource.clip = currentRoom.sceneMusicBank.autoMusic;
            PlayMusicClip(currentRoom.sceneMusicBank.autoMusic);
        }
    }


    public void PlayMusicClip
    (AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
