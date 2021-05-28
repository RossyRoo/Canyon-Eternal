using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimatorHandler : MonoBehaviour
{
    Animator animator;


    public void PlayTargetAnimation(string targetAnim)
    {
        animator = GetComponent<Animator>();
        animator.Play(targetAnim);
    }
}
