using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public SceneChangeManager sceneChangeManager;
    public WeatherSFXPlayer weatherSFXPlayer;

    [Tooltip("1 = Calm. 2 = Sunny. 3 = Breezy. 4 = Rainy. 5 = Acid Rain. 6 = Foggy. 7 = Snowy.")]
    public int currentPattern = 1;
    public ParticleSystem [] weatherPatterns;
    public AudioClip[] weatherSFX;
    public float currentPatternTimeRemaining;
    [Tooltip("Minimum amount of time a weather pattern will play.")]
    public float minPatternDuration;
    [Tooltip("Maximum amount of time a weather pattern will play.")]
    public float maxPatternDuration;
    [Tooltip("Minimum amount of time between weather pattern playing.")]
    public float minPatternBuffer = 4f;
    [Tooltip("Maximum amount of time between weather pattern playing.")]
    public float maxPatternBuffer = 8f;

    [Header("Screen Toner")]
    public Animator weatherTonerAnimator;
    public string[] weatherAnimations;


    public void OnLoadScene()
    {
        if (weatherPatterns[currentPattern - 1].isPlaying)
        {
            if (!sceneChangeManager.currentRoom.possibleWeatherPatterns.Contains(currentPattern))
            {
                float nextBuffer = Random.Range(minPatternBuffer, maxPatternBuffer);

                currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration) + nextBuffer;

                StopPreviousWeatherPattern(true);
                StartCoroutine(PlayNewWeatherPattern(true, 0f));
            }
        }
        else
        {
            currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration);

            currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

            StartCoroutine(PlayNewWeatherPattern(true, 0f));
        }

    }

    private void Update()
    {
        if (currentPatternTimeRemaining > 0)
        {
            currentPatternTimeRemaining -= Time.deltaTime;
        }
        else
        {
            float nextBuffer = Random.Range(minPatternBuffer, maxPatternBuffer);

            currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration) + nextBuffer;

            if(sceneChangeManager.currentRoom.possibleWeatherPatterns.Count > 1) // If room has more than one weather type, take a pause and select a new one
            {
                StopPreviousWeatherPattern(false);

                currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

                StartCoroutine(PlayNewWeatherPattern(false, nextBuffer));
            }
        }
    }


    private void StopPreviousWeatherPattern(bool isHardTransition)
    {
        //STOP ALL ASSOCIATED PARTICLE SYSTEMS
        weatherPatterns[currentPattern - 1].Stop();
        ParticleSystem[] children = weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Stop();
        }

        //PLAY SCREEN TONER
        if(isHardTransition)
        {
            weatherTonerAnimator.Play("Clear");
        }
        else
        {
            weatherTonerAnimator.Play(weatherAnimations[currentPattern - 1] + "ToClear");
        }

        //STOP WEATHER SFX
        weatherSFXPlayer.StopWeatherSFX();
    }


    private IEnumerator PlayNewWeatherPattern(bool isHardTransition, float timeBeforeWeatherChanges)
    {
        yield return new WaitForSeconds(timeBeforeWeatherChanges);

        //PLAY ALL ASSOCIATED PARTICLE SYSTEMS
        if (isHardTransition)
        {
            weatherPatterns[currentPattern - 1].Simulate(weatherPatterns[currentPattern - 1].main.duration * 100);
        }
        weatherPatterns[currentPattern - 1].Play();
        ParticleSystem[] children = weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            if (isHardTransition)
            {
                children[i].Simulate(weatherPatterns[currentPattern - 1].main.duration * 100);
            }
            children[i].Play();
        }

        //PLAY SCREEN TONER
        if (isHardTransition)
        {
            weatherTonerAnimator.Play(weatherAnimations[currentPattern - 1]);
            Debug.Log("Playing animation " + weatherAnimations[currentPattern - 1]);
        }
        else
        {
            weatherTonerAnimator.Play("ClearTo" + weatherAnimations[currentPattern - 1]);
        }

        //PLAY WEATHER SFX
        if (weatherSFX[currentPattern - 1] != null)
        {
            weatherSFXPlayer.PlayWeatherSFX(weatherSFX[currentPattern - 1], 0.1f);
        }
    }

}
