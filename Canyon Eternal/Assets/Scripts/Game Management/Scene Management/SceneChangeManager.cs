using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    public AudioClip transitionAudioClip;

    [Header("Room Management")]
    public Room currentRoom;
    public List<Room> allRoomsInOrder = new List<Room>();

    public int currentBuildIndex;
    public int currentCheckpoint;
    public bool sceneChangeTriggered;

    public void OnLoadScene()
    {
        Instance = this;

        sceneChangeTriggered = false;
    }

    public void FindCurrentRoom(SceneChangeManager sceneChangeManager)
    {
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentRoom = allRoomsInOrder[currentBuildIndex];
        sceneChangeManager.currentRoom = currentRoom;

        if(currentRoom.isCheckpoint)
        {
            currentCheckpoint = currentBuildIndex;
        }
    }

    public IEnumerator ChangeScene(int sceneNum = 999)
    {
        if(!sceneChangeTriggered)
        {
            sceneChangeTriggered = true;

            FindObjectOfType<ScreenFader>().FadeToBlack();

            SFXPlayer.Instance.PlaySFXAudioClip(transitionAudioClip, 0.02f, 0.25f);

            yield return new WaitForSeconds(1f);

            if (sceneNum != 999)
            {
                SceneManager.LoadScene(sceneNum);
            }
            else
            {
                SceneManager.LoadScene(currentBuildIndex + 1);
            }
        }
    }

    public IEnumerator LoadOutsideLastFort(PlayerManager playerManager)
    {
        sceneChangeTriggered = true;

        FindObjectOfType<ScreenFader>().FadeToBlack();

        yield return new WaitForSeconds(1f);

        playerManager.isDead = false;

        CinemachineManager.Instance.FindPlayer(playerManager);
        playerManager.nextSpawnPoint = Vector3.zero;
        SceneManager.LoadScene(currentCheckpoint);
    }

    public void LoadSaveGame()
    {
        SceneManager.LoadScene(currentBuildIndex + 1);
    }
}
