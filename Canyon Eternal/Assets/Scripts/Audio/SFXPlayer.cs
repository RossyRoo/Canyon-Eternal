using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFXAudioClip
        (AudioClip audioClip, float volume = 1f, float delay = 0f)
    {
        StartCoroutine(DelaySound(audioClip, Camera.main.transform.position, volume, delay));
    }

    private IEnumerator DelaySound(AudioClip audioClip, Vector3 soundPosition, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource.PlayClipAtPoint(audioClip, soundPosition, volume);
    }
}
