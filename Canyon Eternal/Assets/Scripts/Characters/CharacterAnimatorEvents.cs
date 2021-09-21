using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorEvents : MonoBehaviour
{
    public CharacterManager characterManager;

    public void EnableVulnerabilityToBlock()
    {
        characterManager.isVulnerableToParry = true;
    }

    public void DisableVulnerabilityToBlock()
    {
        characterManager.isVulnerableToParry = false;
    }
}
