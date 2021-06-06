using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    GenericAnimatorHandler blackFaderAnimatorHandler;
    DontDestroy dontDestroy;
    public AudioClip transitionAudioClip;

    public bool sceneChangeTriggered;

    public void OnLoadScene()
    {
        dontDestroy = GetComponent<DontDestroy>();

        Instance = this;

        sceneChangeTriggered = false;

        blackFaderAnimatorHandler = FindObjectOfType<GenericAnimatorHandler>();
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeFromBlack");
    }

    public IEnumerator ChangeScene(int sceneNum = 999)
    {
        if(!sceneChangeTriggered)
        {
            sceneChangeTriggered = true;

            blackFaderAnimatorHandler.PlayTargetAnimation("FadeToBlack");
            SFXPlayer.Instance.PlaySFXAudioClip(transitionAudioClip, 1f, 0.25f);

            yield return new WaitForSeconds(1f);

            if (sceneNum != 999)
            {
                SceneManager.LoadScene(sceneNum);
            }
            else
            {
                SceneManager.LoadScene(dontDestroy.currentBuildIndex + 1);
            }
        }
    }

    public IEnumerator LoadOutsideLastFort(PlayerManager playerManager)
    {
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeToBlack");

        yield return new WaitForSeconds(1f);

        playerManager.isDead = false;

        CinemachineManager.Instance.FindPlayer(playerManager);
        SceneManager.LoadScene(dontDestroy.currentBuildIndex);
    }

    public void LoadSaveGame()
    {
        SceneManager.LoadScene(dontDestroy.currentBuildIndex + 1);
    }
}
