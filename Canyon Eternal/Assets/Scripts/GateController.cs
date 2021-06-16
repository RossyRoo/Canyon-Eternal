using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    PlayerManager playerManager;

    [Tooltip("Drop all gates in here")]
    public GameObject[] gates;

    [Tooltip("Drop all enemies within gates here")]
    public EnemyManager[] enemiesWithinGates;

    public AudioClip gateSFX;

    bool gateTriggered;
    bool gateComplete;

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
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].GetComponent<BoxCollider2D>().enabled = true;
            gates[i].GetComponentInChildren<Animator>().Play("Close");
        }

        gateTriggered = true;

        SFXPlayer.Instance.PlaySFXAudioClip(gateSFX, 0.5f);
    }

    public void OpenAllGates()
    {
        if(!gateComplete)
        {
            for (int i = 0; i < gates.Length; i++)
            {
                gates[i].GetComponent<BoxCollider2D>().enabled = false;
                gates[i].GetComponentInChildren<Animator>().Play("Open");
            }

            gateComplete = true;

            SFXPlayer.Instance.PlaySFXAudioClip(gateSFX, 0.5f);
        }

    }

}
