using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance { get; private set; }

    public GameObject oneShotAudioPrefab;

    public void OnLoadScene()
    {
        if(Instance == null)
        {
            Instance = this;

        }
    }

    public void PlaySFXAudioClip
        (AudioClip audioClip, float volume = 0.3f, float delay = 0f)
    {
        StartCoroutine(MyPlayClipAtPoint(audioClip, volume, delay));
    }

    private IEnumerator MyPlayClipAtPoint(AudioClip audioClip, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject oneShotAudioGO = Instantiate(oneShotAudioPrefab, Camera.main.transform.position, Quaternion.identity);
        oneShotAudioGO.transform.parent = Camera.main.transform;

        AudioSource oneShotAudioSource = oneShotAudioGO.GetComponent<AudioSource>();
        oneShotAudioSource.clip = audioClip;
        oneShotAudioSource.volume = volume;
        oneShotAudioSource.Play();
        Destroy(oneShotAudioGO, audioClip.length);
    }
}
