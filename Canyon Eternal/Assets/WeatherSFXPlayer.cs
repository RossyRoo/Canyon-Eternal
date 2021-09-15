using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSFXPlayer : MonoBehaviour
{
    public AudioSource audioSource;


    public void PlayWeatherSFX
    (AudioClip audioClip, float volume = 0.3f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopWeatherSFX()
    {
        audioSource.Stop();
    }
}
