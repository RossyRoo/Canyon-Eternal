using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> gates;
    PlayerManager playerManager;
    public AudioClip gateSFX;
    bool gateTriggered;
    bool gateComplete;

    [Header("Encounter")]
    public EnemyManager[] enemiesWithinGates;
    public AudioClip encounterMusic;

    private void Awake()
    {
        BoxCollider2D[] children = GetComponentsInChildren<BoxCollider2D>();

        for (int i = 0; i < children.Length; i++)
        {
            gates.Add(children[i].gameObject);
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
                && enemiesWithinGates.Length > 0)
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
            if (!playerManager.isInCombat)
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
