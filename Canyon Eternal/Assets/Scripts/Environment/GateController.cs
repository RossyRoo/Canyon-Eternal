using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("ENCOUNTER")]
    public EnemyManager[] encounterEnemies;
    public AudioClip encounterMusic;

    public List<GameObject> gates;
    PlayerManager playerManager;
    public AudioClip gateSFX;
    bool gateTriggered;
    bool gateComplete;

    private void Awake()
    {
        BoxCollider2D[] children = GetComponentsInChildren<BoxCollider2D>();

        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].gameObject.layer != LayerMask.NameToLayer("AggroWall"))
            {
                gates.Add(children[i].gameObject);
            }
        }
    }

    private void Update()
    {
        CheckArea();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!gateTriggered)
        {
            if (collision.GetComponent<PlayerManager>()
                && encounterEnemies[0] != null)
            {
                playerManager = collision.GetComponent<PlayerManager>();
                CloseAllGates();
            }
        }
    }

    private void CheckArea()
    {
        if(gateTriggered)
        {
            if (encounterEnemies[0] == null)
            {
                OpenAllGates();
            }
        }
    }

    public void CloseAllGates()
    {
        for (int i = 0; i < gates.Count; i++)
        {
            gates[i].GetComponent<BoxCollider2D>().enabled = true;
            gates[i].GetComponentInChildren<Animator>().Play("Close");
        }

        gateTriggered = true;

        SFXPlayer.Instance.PlaySFXAudioClip(gateSFX, 0.5f);
        MusicPlayer.Instance.PlayMusicClip(encounterMusic);
    }

    public void OpenAllGates()
    {
        if(!gateComplete)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                gates[i].GetComponent<BoxCollider2D>().enabled = false;
                gates[i].GetComponentInChildren<Animator>().Play("Open");
            }

            gateComplete = true;

            SFXPlayer.Instance.PlaySFXAudioClip(gateSFX, 0.5f);
            MusicPlayer.Instance.PlayRoomMusic(playerManager.GetComponentInParent<SceneChangeManager>().currentRoom);
        }

    }

}
