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
    PlayerSpellHandler playerSpellHandler;
    PlayerAnimatorHandler playerAnimatorHandler;

    public GameObject interactionPopupGO;
    public GameObject itemPopupGO;
    public InteractableUI interactableUI;
    public LayerMask interactableLayers;

    public Vector3 nextSpawnPoint;

    private void Awake()
    {
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerSpellHandler = GetComponent<PlayerSpellHandler>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        UpdateAnimatorParameters();
        inputManager.HandleAllInputs();
        playerStats.RegenerateStamina();
        CheckForInteractable();
        playerMeleeHandler.CheckToDespawnMelee();
        playerSpellHandler.TickSpellChargeTimer();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            playerLocomotion.HandleDash();
        }
        playerLocomotion.HandleMovement();
        playerMeleeHandler.AdjustAttackMomentum();
    }

    private void LateUpdate()
    {
        ResetInputLate();
    }

    private void UpdateAnimatorParameters()
    {
        isInteracting = animator.GetBool("isInteracting");
        isAttacking = animator.GetBool("isAttacking");

        playerMeleeHandler.comboNumber = animator.GetInteger("comboNumber");
        playerMeleeHandler.comboWasHit = animator.GetBool("comboWasHit");
        playerMeleeHandler.comboWasMissed = animator.GetBool("comboWasMissed");

        if(isChargingSpell || isFalling)
        {
            isLockedInPlace = true;
        }
        else
        {
            isLockedInPlace = false;
        }
    }

    private void ResetInputLate()
    {
        inputManager.melee_Input = false;
        inputManager.dash_Input = false;
        inputManager.heal_Input = false;
        inputManager.interact_Input = false;
        inputManager.block_Input = false;
        inputManager.chargeSpell_Input = false;
        inputManager.castSpell_Input = false;
    }

    public IEnumerator HandleDeathCoroutine()
    {
        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerAnimatorHandler.PlayTargetAnimation("Death", true);
        yield return new WaitForSeconds(1f);
        isDead = false;
        //Drop fragments
        //Reload from fort
        playerStats.SetStartingStats();
        StartCoroutine(SceneChangeManager.Instance.LoadOutsideLastFort(this));
    }

    public void OnLoadScene(Room currentRoom)
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        animator.SetBool("isInteracting", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

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
            lastMoveDirection, 5f, interactableLayers);



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
                        hit.collider.GetComponent<Interactable>().Interact(this, playerStats);
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
