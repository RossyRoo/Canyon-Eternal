using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorHandler : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.Play(targetAnim);
    }

    public void UpdateFloatAnimationValues(float moveX, float moveY, bool isMoving)
    {
        animator.SetFloat("moveX_f", moveX);
        animator.SetFloat("moveY_f", moveY);
        animator.SetBool("isMoving", isMoving);
    }

    public void UpdateIntAnimationValues(float moveX, float moveY, bool isMoving)
    {
        animator.SetInteger("moveX", Mathf.FloorToInt(moveX));
        animator.SetInteger("moveY", Mathf.FloorToInt(moveY));
        animator.SetBool("isMoving", isMoving);
    }
}
