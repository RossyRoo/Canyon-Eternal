using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorHandler : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public CharacterManager characterManager;

    public SpriteRenderer torsoSpriteRenderer;

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

}
