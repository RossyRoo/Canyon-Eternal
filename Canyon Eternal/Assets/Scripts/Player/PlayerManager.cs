using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    //Player Components
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    Animator animator;

    //Game Components
    public bool dashFlag;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        isInteracting = animator.GetBool("isInteracting");

        inputManager.HandleAllInputs();
        playerStats.RegenerateStamina();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleMovement();

        if(dashFlag)
        {
            playerLocomotion.HandleDash();
        }
    }

    private void LateUpdate()
    {
        ResetAllInputs();
    }

    private void ResetAllInputs()
    {
        inputManager.melee_Input = false;
        inputManager.dash_Input = false;
        inputManager.heal_Input = false;
        //inputManager.guard_Input = false;
    }

    public IEnumerator HandleDeathCoroutine()
    {
        yield return new WaitForSeconds(deathTimeBuffer);
        //Drop fragments
        //Reload from fort
    }

    public void OnLoadScene()
    {
        transform.position = GameObject.FindGameObjectWithTag("Entrance").transform.position;

        if(myWall != null)
        {
            Destroy(myWall.gameObject);
        }

        GenerateTrackingWall();
    }

}
