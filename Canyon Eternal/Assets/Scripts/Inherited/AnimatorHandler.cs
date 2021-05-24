using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
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

    public void UpdateMoveAnimationValues(float moveX, float moveY, bool isMoving)
    {
        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);
        animator.SetBool("isMoving", isMoving);
    }
}
