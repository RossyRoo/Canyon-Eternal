using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimatorHandler : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(string targetAnim)
    {
        animator.Play(targetAnim);
    }
}
