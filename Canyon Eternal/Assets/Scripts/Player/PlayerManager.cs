using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    [HideInInspector]public Animator animator;
    PlayerMeleeHandler playerMeleeHandler;
    PlayerSpellHandler playerSpellHandler;
    PlayerProgression playerProgression;

    public bool isInteractingWithUI;

    [Header("Interactions")]
    public GameObject interactionPopupGO;
    public GameObject itemPopupGO;

    public InteractableUI interactableUI;
    public LayerMask interactableLayers;

    public AreaNameText areaNameText;

    public Vector3 nextSpawnPoint;
    public int nextDoorNum;

    public List<EnemyManager> enemiesEngaged;

    public SpriteRenderer[] bodySprites;

    private void Awake()
    {
        playerProgression = GetComponentInChildren<PlayerProgression>();
        playerMeleeHandler = GetComponent<PlayerMeleeHandler>();
        playerSpellHandler = GetComponent<PlayerSpellHandler>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GenerateTrackingWall();
    }

    private void Update()
    {
        UpdateAnimatorParameters();
        inputManager.HandleAllInputs();
        playerStats.RegenerateStamina();
        CheckForInteractable();
        playerSpellHandler.TickSpellChargeTimer();
        EnemyCheck();
        playerMeleeHandler.TickAttackCooldown();
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
        isMoving = animator.GetBool("isMoving");
    }

    private void ResetInputLate()
    {
        inputManager.melee_Input = false;
        inputManager.dash_Input = false;
        inputManager.heal_Input = false;
        inputManager.item_Input = false;
        inputManager.interact_Input = false;
        inputManager.block_Input = false;
        inputManager.chargeSpell_Input = false;
        inputManager.castSpell_Input = false;

        inputManager.openMenu_Input = false;
        inputManager.cycleMenuLeft_Input = false;
        inputManager.cycleMenuRight_Input = false;
        inputManager.cycleSubmenuLeft_Input = false;
        inputManager.cycleSubmenuRight_Input = false;
    }

    public void OnLoadScene(Room currentRoom)
    {
        animator = GetComponentInChildren<Animator>();
        playerProgression = GetComponentInChildren<PlayerProgression>();
        SwitchSpriteVisibility(true);
        animator.SetBool("isInteracting", false);
        isFalling = false;

        Door[] doorsInRoom = FindObjectsOfType<Door>();
        for (int i = 0; i < doorsInRoom.Length; i++)
        {
            if (int.Parse(doorsInRoom[i].name) == nextDoorNum)
            {
                transform.position = doorsInRoom[i].transform.position;
                
                if(doorsInRoom[i].newAreaNameText != null)
                {
                    StartCoroutine(areaNameText.ShowAreaName(doorsInRoom[i].newAreaNameText));
                }
            }
        }

        if (!playerProgression.roomsDiscovered.Contains(currentRoom))
        {
            playerProgression.roomsDiscovered.Add(currentRoom);

            if(currentRoom.isFastTravelPoint && !playerProgression.fastTravelLocationsDiscovered.Contains(currentRoom))
            {
                playerProgression.fastTravelLocationsDiscovered.Add(currentRoom);
            }
        }

    }

    #region Area Checks

    private void CheckForInteractable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            lastMoveDirection, 5f, interactableLayers);

            if (hit && !isInteractingWithUI)
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactionPopupGO.SetActive(true);

                    if (inputManager.interact_Input && !isInteracting)
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

    private void EnemyCheck()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, playerStats.characterData.detectionRadius);

        List<EnemyManager> enemiesEngaged = new List<EnemyManager>();

        int enemiesDetected = 0;

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i].GetComponent<EnemyManager>())
            {
                enemiesDetected++;
            }
        }

        if (enemiesDetected < 1 && enemiesEngaged.Count < 1)
        {
            isInCombat = false;
        }
        else
        {
            isInCombat = true;
        }

    }

    #endregion

    #region Handle Conversation

    public void EnterConversationState()
    {
        if(isDashing)
        {
            playerLocomotion.StopDash();
        }
        isInteractingWithUI = true;
        animator.SetBool("isInteracting", true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void ExitConversationState()
    {
        isInteractingWithUI = false;
        animator.SetBool("isInteracting", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    #endregion

    public void SwitchSpriteVisibility(bool isVisible)
    {
        for (int i = 0; i < bodySprites.Length; i++)
        {
            bodySprites[i].enabled = isVisible;
        }
    }


}

