using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public SceneChangeManager sceneChangeManager;
    public WeatherSFXPlayer weatherSFXPlayer;

    [Tooltip("1 = Calm. 2 = Sunny. 3 = Breezy. 4 = Rainy. 5 = Acid Rain. 6 = Foggy. 7 = Snowy.")]
    public int currentPattern = 1;
    public ParticleSystem [] weatherSystems;
    public AudioClip[] weatherSFX;
    public float currentPatternTimeRemaining;
    public float minPatternDuration;
    public float maxPatternDuration;
    float minPatternBuffer = 5f;
    float maxPatternBuffer = 8f;


    public void OnLoadScene()
    {
        if (weatherSystems[currentPattern - 1].isPlaying)
        {
            if (!sceneChangeManager.currentRoom.possibleWeatherPatterns.Contains(currentPattern))
            {
                float nextBuffer = Random.Range(minPatternBuffer, maxPatternBuffer);

                currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration) + nextBuffer;

                HardStopPreviousWeatherPattern();
                HardPlayNewWeatherPattern();
            }
        }
        else
        {
            float nextBuffer = Random.Range(minPatternBuffer, maxPatternBuffer);

            currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration) + nextBuffer;
            HardPlayNewWeatherPattern();
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
                SoftStopPreviousWeatherPattern();
                Invoke("SoftPlayNewWeatherPattern", nextBuffer);
            }
        }
    }


    #region Soft Weather Change
    private void SoftPlayNewWeatherPattern()
    {
        currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

        weatherSystems[currentPattern - 1].Play();

        ParticleSystem[] children= weatherSystems[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Play();
        }

        if (weatherSFX[currentPattern - 1] != null)
        {
            weatherSFXPlayer.PlayWeatherSFX(weatherSFX[currentPattern - 1], 0.1f);
        }

        //Debug.Log("Soft playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");

    }

    private void SoftStopPreviousWeatherPattern()
    {
        if (weatherSystems[currentPattern - 1].isPlaying)
        {
            weatherSystems[currentPattern - 1].Stop();
            ParticleSystem[] children = weatherSystems[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < children.Length; i++)
            {
                 children[i].Stop();
            }
        }

        weatherSFXPlayer.StopWeatherSFX();

        //Debug.Log("Soft stopping " + weatherSystems[currentPattern - 1].name);
    }
    #endregion

    #region Hard Weather Change
    private void HardStopPreviousWeatherPattern()
    {
        weatherSystems[currentPattern - 1].Clear();
        weatherSystems[currentPattern - 1].Stop();
        ParticleSystem[] children = weatherSystems[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Clear();
            children[i].Stop();
        }

        weatherSFXPlayer.StopWeatherSFX();

        //Debug.Log("Hard stopping " + weatherSystems[currentPattern - 1].name);
    }


    private void HardPlayNewWeatherPattern()
    {
        currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

        weatherSystems[currentPattern - 1].Simulate(weatherSystems[currentPattern - 1].main.duration * 100);
        weatherSystems[currentPattern - 1].Play();
        ParticleSystem[] children = weatherSystems[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Simulate(weatherSystems[currentPattern - 1].main.duration * 100);
            children[i].Play();
        }

        if (weatherSFX[currentPattern - 1] != null)
        {
            weatherSFXPlayer.PlayWeatherSFX(weatherSFX[currentPattern - 1], 0.1f);
        }

        //Debug.Log("Hard playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");
    }

    #endregion
}
