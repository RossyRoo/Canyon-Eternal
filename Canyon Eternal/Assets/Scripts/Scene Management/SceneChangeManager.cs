using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    GenericAnimatorHandler blackFaderAnimatorHandler;
    public AudioClip transitionAudioClip;

    float transitionScreenBuffer = 1f;
    public int currentBuildIndex;

    private void Start()
    {
        if (!GetComponentInParent<DontDestroy>())
        {
            OnLoadScene();
        }
    }

    public void OnLoadScene()
    {
        Instance = this;

        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        blackFaderAnimatorHandler = FindObjectOfType<GenericAnimatorHandler>();
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeFromBlack");
    }

    public IEnumerator ChangeScene(int sceneNum = 999)
    {
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeToBlack");

        SFXPlayer.Instance.PlaySFXAudioClip(transitionAudioClip, 1f, 0.25f);
        yield return new WaitForSeconds(transitionScreenBuffer);

        if (sceneNum != 999)
        {
            SceneManager.LoadScene(sceneNum);
        }
        else
        {
            SceneManager.LoadScene(currentBuildIndex + 1);
        }
    }

    public void LoadOutsideLastFort()
    {
        SceneManager.LoadScene(currentBuildIndex);
    }

    public void LoadSaveGame()
    {
        SceneManager.LoadScene(currentBuildIndex + 1);
    }
}
