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
    public int nextDoorNum;

    public List<EnemyManager> enemiesEngaged;


    private void Awake()
    {
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
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

    public IEnumerator HandleDeathCoroutine(string deathAnimation = "Death")
    {
        isDead = true;
        playerAnimatorHandler.PlayTargetAnimation(deathAnimation, true);
        yield return new WaitForSeconds(1f);
        //Drop fragments
        playerStats.SetStartingStats();
        StartCoroutine(SceneChangeManager.Instance.LoadOutsideLastFort(this));
    }

    public void OnLoadScene(Room currentRoom)
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("isInteracting", false);


        Door[] doorsInRoom = FindObjectsOfType<Door>();
        for (int i = 0; i < doorsInRoom.Length; i++)
        {
            if (int.Parse(doorsInRoom[i].name) == nextDoorNum)
            {
                transform.position = doorsInRoom[i].transform.position;
            }
        }

    }

    #region Area Checks

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
        isConversing = true;
        isInteracting = true;
        playerAnimatorHandler.UpdateFloatAnimationValues(lastMoveDirection.x, lastMoveDirection.y, false);
    }

    public void ExitConversationState()
    {
        isConversing = false;
        isInteracting = false;
    }

    #endregion


}
