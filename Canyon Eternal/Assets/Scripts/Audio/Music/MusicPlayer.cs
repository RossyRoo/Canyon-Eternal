using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    AudioSource audioSource;
    public Room backupRoom;

    private void Start()
    {
        if(!GetComponentInParent<DontDestroy>())
        {
            audioSource = GetComponent<AudioSource>();
            PlaySceneMusic(backupRoom);
        }
    }

    public void OnLoadScene(Room currentRoom)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        PlaySceneMusic(currentRoom);
    }

    private void PlaySceneMusic(Room currentRoom)
    {
        if (currentRoom.roomAudio != null
            && currentRoom.roomAudio != audioSource.clip
            || !audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = currentRoom.roomAudio;
            PlayMusicClip(currentRoom.roomAudio);
        }
    }


    public void PlayMusicClip
    (AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
