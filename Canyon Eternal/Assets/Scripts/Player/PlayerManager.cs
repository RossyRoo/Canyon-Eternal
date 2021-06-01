using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    Animator animator;
    PlayerMeleeHandler playerMeleeHandler;

    public GameObject interactionPopupGO;
    public GameObject itemPopupGO;
    public InteractableUI interactableUI;
    public LayerMask interactableLayers;

    //Game Components
    public bool dashFlag;

    public Vector3 nextSpawnPoint;

    private void Awake()
    {
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        isInteracting = animator.GetBool("isInteracting");
        isAttacking = animator.GetBool("isAttacking");

        inputManager.HandleAllInputs();
        playerStats.RegenerateStamina();
        CheckForInteractable();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleMovement();
        playerMeleeHandler.AdjustAttackMomentum();

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
        inputManager.interact_Input = false;
        inputManager.guard_Input = false;
    }

    public IEnumerator HandleDeathCoroutine()
    {
        yield return new WaitForSeconds(deathTimeBuffer);
        //Death Anim
        //Drop fragments
        //Reload from fort
        playerStats.SetStartingStats();
        SceneChangeManager.Instance.LoadOutsideLastFort();
    }

    public void OnLoadScene(RoomData currentRoom)
    {
        if (nextSpawnPoint == Vector3.zero)
        {
            nextSpawnPoint = currentRoom.spawnPoints[0];
        }
        transform.position = nextSpawnPoint;


        if(myWall != null)
        {
            Destroy(myWall.gameObject);
        }

        GenerateTrackingWall();
    }

    private void CheckForInteractable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            lastMoveDirection, 10f, interactableLayers);

        /*Debug.DrawRay(transform.position,
            playerLocomotion.lastMoveDirection * 10f, Color.red);*/

            if (hit)
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactionPopupGO.SetActive(true);

                    if (inputManager.interact_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
            else
            {
                if (interactionPopupGO != null)
                {
                    interactionPopupGO.SetActive(false);
                }

                if (itemPopupGO != null && inputManager.interact_Input == true)
                {
                    itemPopupGO.SetActive(false);
                }
            }

    }


}
