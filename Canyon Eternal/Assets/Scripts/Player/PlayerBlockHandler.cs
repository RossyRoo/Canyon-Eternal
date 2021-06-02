using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{

    PlayerAnimatorHandler playerAnimatorHandler;

    private void Awake()
    {
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
    }

    public void HandleBlock()
    {
        playerAnimatorHandler.PlayTargetAnimation("Block", true);
        Debug.Log("Do a block");
    }
}
