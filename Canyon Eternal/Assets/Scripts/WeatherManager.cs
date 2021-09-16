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
    public float minPatternDuration;
    public float maxPatternDuration;
    float minPatternBuffer = 5f;
    float maxPatternBuffer = 8f;

    [Header("Screen Toner")]
    public Animator weatherTonerAnimator;
    public string [] clearToWeatherAnimations;
    public string [] weatherToClearAnimations;
    public string[] hardWeatherAnimations;


    public void OnLoadScene()
    {
        if (weatherPatterns[currentPattern - 1].isPlaying)
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

        //PLAY ALL ASSOCIATED PARTICLE SYSTEMS
        weatherPatterns[currentPattern - 1].Play();
        ParticleSystem[] children= weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Play();
        }

        //PLAY SCREEN TONER
        weatherTonerAnimator.Play(clearToWeatherAnimations[currentPattern - 1]);

        //PLAY WEATHER SFX
        if (weatherSFX[currentPattern - 1] != null)
        {
            weatherSFXPlayer.PlayWeatherSFX(weatherSFX[currentPattern - 1], 0.1f);
        }

        //Debug.Log("Soft playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");

    }

    private void SoftStopPreviousWeatherPattern()
    {
        //STOP ALL ASSOCIATED PARTICLE SYSTEMS
        if (weatherPatterns[currentPattern - 1].isPlaying)
        {
            weatherPatterns[currentPattern - 1].Stop();
            ParticleSystem[] children = weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].Clear();
                children[i].Stop();
            }
        }

        //PLAY SCREEN TONER
        weatherTonerAnimator.Play(weatherToClearAnimations[currentPattern - 1]);

        //STOP WEATHER SFX
        weatherSFXPlayer.StopWeatherSFX();

        //Debug.Log("Soft stopping " + weatherSystems[currentPattern - 1].name);
    }
    #endregion

    #region Hard Weather Change
    private void HardStopPreviousWeatherPattern()
    {
        //STOP ALL ASSOCIATED PARTICLE SYSTEMS
        weatherPatterns[currentPattern - 1].Clear();
        weatherPatterns[currentPattern - 1].Stop();
        ParticleSystem[] children = weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Clear();
            children[i].Stop();
        }

        //PLAY SCREEN TONER
        weatherTonerAnimator.Play(hardWeatherAnimations[currentPattern - 1]);

        //STOP WEATHER SFX
        weatherSFXPlayer.StopWeatherSFX();

        //Debug.Log("Hard stopping " + weatherSystems[currentPattern - 1].name);
    }


    private void HardPlayNewWeatherPattern()
    {
        currentPattern = sceneChangeManager.currentRoom.possibleWeatherPatterns[Random.Range(0, sceneChangeManager.currentRoom.possibleWeatherPatterns.Count)];

        //PLAY ALL ASSOCIATED PARTICLE SYSTEMS
        weatherPatterns[currentPattern - 1].Simulate(weatherPatterns[currentPattern - 1].main.duration * 100);
        weatherPatterns[currentPattern - 1].Play();
        ParticleSystem[] children = weatherPatterns[currentPattern - 1].GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].Simulate(weatherPatterns[currentPattern - 1].main.duration * 100);
            children[i].Play();
        }

        //PLAY SCREEN TONER
        weatherTonerAnimator.Play(hardWeatherAnimations[currentPattern - 1]);

        //PLAY WEATHER SFX
        if (weatherSFX[currentPattern - 1] != null)
        {
            weatherSFXPlayer.PlayWeatherSFX(weatherSFX[currentPattern - 1], 0.1f);
        }

        //Debug.Log("Hard playing " + weatherSystems[currentPattern - 1].name + " for " + currentPatternTimeRemaining + " seconds.");
    }

    #endregion
}
