using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public SceneChangeManager sceneChangeManager;

    [Tooltip("1 = Calm. 2 = Sunny. 3 = Breezy. 4 = Rainy. 5 = Acid Rain. 6 = Foggy. 7 = Snowy.")]
    public int currentPattern = 1;
    public ParticleSystem [] weatherSystems;
    public float currentPatternTimeRemaining;
    public float minPatternDuration;
    public float maxPatternDuration;
    float minPatternBuffer = 3f;
    float maxPatternBuffer = 6f;


    public void OnLoadScene()
    {
        if (weatherSystems[currentPattern - 1].isPlaying)
        {
            Debug.Log("Weather is playing");

            if (!sceneChangeManager.currentRoom.possibleWeatherPatterns.Contains(currentPattern - 1))
            {
                Debug.Log("Need to hard change");
                float nextBuffer = Random.Range(minPatternBuffer, maxPatternBuffer);

                currentPatternTimeRemaining = Random.Range(minPatternDuration, maxPatternDuration) + nextBuffer;

                HardStopPreviousWeatherPattern();
                HardPlayNewWeatherPattern();
            }
        }
        else
        {
            SoftPlayNewWeatherPattern();
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

        Debug.Log("Soft playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");

    }

    private void SoftStopPreviousWeatherPattern()
    {
        if (weatherSystems[currentPattern - 1].isPlaying)
        {
            weatherSystems[currentPattern - 1].Stop();
        }
        Debug.Log("Soft stopping " + weatherSystems[currentPattern - 1].name);
    }
    #endregion

    #region Hard Weather Change
    private void HardStopPreviousWeatherPattern()
    {
        weatherSystems[currentPattern - 1].Clear();
        weatherSystems[currentPattern - 1].Stop();
        Debug.Log("Hard stopping " + weatherSystems[currentPattern - 1].name);
    }


    private void HardPlayNewWeatherPattern()
    {
        currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

        weatherSystems[currentPattern - 1].Simulate(weatherSystems[currentPattern - 1].main.duration * 2);
        weatherSystems[currentPattern - 1].Play();

        Debug.Log("Hard playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");

    }

    #endregion
}
