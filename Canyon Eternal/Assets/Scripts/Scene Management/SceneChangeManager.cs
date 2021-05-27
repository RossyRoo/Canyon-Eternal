using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    public float loadTime =1f;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        //Fade to black
        //Wait loadtime
        //Fade from black
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(sceneName);
    }
}
