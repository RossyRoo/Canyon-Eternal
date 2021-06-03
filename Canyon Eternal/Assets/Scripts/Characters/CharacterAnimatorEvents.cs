using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorEvents : MonoBehaviour
{
    public CharacterManager characterManager;

    public void EnableVulnerabilityToBlock()
    {
        characterManager.isVulnerableToBlock = true;
    }

    public void DisableVulnerabilityToBlock()
    {
        characterManager.isVulnerableToBlock = false;
    }
}
