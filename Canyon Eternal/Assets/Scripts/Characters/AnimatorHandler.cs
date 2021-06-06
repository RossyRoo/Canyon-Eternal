using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorHandler : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public CharacterManager characterManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterManager = GetComponentInParent<CharacterManager>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.Play(targetAnim);
    }

    public void UpdateFloatAnimationValues(float moveX, float moveY, bool isMoving)
    {
        if(!characterManager.isLockedInPlace)
        {
            animator.SetFloat("moveX_Float", moveX);
            animator.SetFloat("moveY_Float", moveY);
            animator.SetBool("isMoving", isMoving);
        }

    }

    public void UpdateIntAnimationValues(float moveX, float moveY, bool isMoving)
    {
        if (!characterManager.isLockedInPlace)
        {
            animator.SetInteger("moveX_Int", Mathf.FloorToInt(moveX));
            animator.SetInteger("moveY_Int", Mathf.FloorToInt(moveY));
            animator.SetBool("isMoving", isMoving);
        }
    }
}
