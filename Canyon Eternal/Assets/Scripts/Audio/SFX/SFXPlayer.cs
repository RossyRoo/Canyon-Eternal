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
        (AudioClip audioClip, float volume = 0.4f, float delay = 0f)
    {
        Debug.Log(Camera.main.transform.position);
        StartCoroutine(MyPlayClipAtPoint(audioClip, Camera.main.transform.position, volume, delay));
    }

    private IEnumerator MyPlayClipAtPoint(AudioClip audioClip, Vector3 soundPosition, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject oneShotAudioGO = Instantiate(oneShotAudioPrefab, Camera.main.transform.position, Quaternion.identity);
        oneShotAudioGO.transform.parent = Camera.main.transform;

        AudioSource oneShotAudioSource = oneShotAudioGO.GetComponent<AudioSource>();
        oneShotAudioSource.clip = audioClip;
        oneShotAudioSource.Play();
        Destroy(oneShotAudioGO, audioClip.length);
    }
}
