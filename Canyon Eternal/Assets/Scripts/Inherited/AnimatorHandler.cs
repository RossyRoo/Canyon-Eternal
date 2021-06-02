﻿using System.Collections;
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
        animator.SetFloat("moveX_Float", moveX);
        animator.SetFloat("moveY_Float", moveY);
        animator.SetBool("isMoving", isMoving);
    }

    public void UpdateIntAnimationValues(float moveX, float moveY, bool isMoving)
    {
        animator.SetInteger("moveX_Int", Mathf.FloorToInt(moveX));
        animator.SetInteger("moveY_Int", Mathf.FloorToInt(moveY));
        animator.SetBool("isMoving", isMoving);
    }
}
