using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerLocomotion playerLocomotion;

    //INPUT DECLARATIONS
    public Vector2 moveInput;
    public Vector2 lastMoveInput;

    public bool melee_Input;
    public bool dash_Input;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Start()
    {
        lastMoveInput = new Vector2(0, -1);
    }

    private void OnEnable()
    {
        if(playerControls==null)
        {
            playerControls = new PlayerControls();

            //MOVEMENT
            playerControls.PlayerMovement.Move.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Move.canceled += i => lastMoveInput = moveInput;
            playerControls.PlayerMovement.Move.canceled += i => moveInput = Vector2.zero;

            //ATTACKING
            playerControls.PlayerActions.Melee.performed += i => melee_Input = true;

            //DASH
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMeleeInput();
        HandleDashInput();
    }

    private void HandleMeleeInput()
    {
        if(melee_Input)
        {
            if (playerManager.isInteracting)
                return;

            playerMeleeHandler.HandleMeleeAttack();
        }
    }

    private void HandleDashInput()
    {
        if(dash_Input)
        {
            if (playerManager.isInteracting)
                return;

            playerManager.dashFlag = true;
        }
    }

}
