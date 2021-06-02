using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorEvents : MonoBehaviour
{
    public CharacterManager characterManager;


    private void Awake()
    {
        characterManager = GetComponentInParent<CharacterManager>();
    }

    public void EnableVulnerabilityToBlock()
    {
        //Do this right as enemy's attack opens its damage collider
        characterManager.isVulnerableToBlock = true;
    }

    public void DisableVulnerabilityToBlock()
    {
        //Do this right as enemy's attack is at its apex
        characterManager.isVulnerableToBlock = false;
    }
}
