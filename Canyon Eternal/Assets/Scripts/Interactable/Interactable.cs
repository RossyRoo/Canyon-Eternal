using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableText;

    private void Awake()
    {
        interactableText = interactableText + " [x]";
    }

    public virtual void Interact(PlayerManager playerManager, PlayerStats playerStats)
    {
        Debug.Log("Player interacted with " + gameObject.name);
    }
}
