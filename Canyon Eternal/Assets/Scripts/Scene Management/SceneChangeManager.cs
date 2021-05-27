using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    GenericAnimatorHandler blackFaderAnimatorHandler;

    public float loadTime =0.5f;
    public int currentBuildIndex;

    public void OnLoadScene()
    {
        Instance = this;

        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        blackFaderAnimatorHandler = GetComponentInChildren<GenericAnimatorHandler>();
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeFromBlack");
    }

    public IEnumerator ChangeScene(int sceneNum = 999)
    {
        blackFaderAnimatorHandler.PlayTargetAnimation("FadeToBlack");
        yield return new WaitForSeconds(loadTime);

        if (sceneNum != 999)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentBuildIndex + 1);
        }
    }
}
