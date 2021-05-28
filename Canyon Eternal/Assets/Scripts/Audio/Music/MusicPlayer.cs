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
            audioSource = GetComponent<AudioSource>();
            AutoplayMusic();
        }
    }

    public void OnLoadScene()
    {
        if (Instance == null)
        {
            audioSource = GetComponent<AudioSource>();
            Instance = this;
            AutoplayMusic();
        }
    }

    private void AutoplayMusic()
    {
        SceneMusicBank sceneMusicBank = FindObjectOfType<SceneMusicBank>();

        if(sceneMusicBank.autoMusic != null)
        {
            PlayMusicAudioClip(sceneMusicBank.autoMusic);
        }
    }

    public void PlayMusicAudioClip
    (AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
