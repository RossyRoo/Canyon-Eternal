using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public SceneChangeManager sceneChangeManager;

    public void OnClickPlayButton()
    {
        sceneChangeManager.LoadSavedGame();
        //Load Save Game
    }
}
