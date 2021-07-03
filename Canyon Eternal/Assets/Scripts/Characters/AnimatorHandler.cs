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

    public void UpdateSprite(float moveX, float moveY, CharacterData characterData)
    {
        if(moveX == 0 && moveY == -1)
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[0];
        }
        else if((moveX == -1 && moveY == -1))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[1];
        }
        else if ((moveX == -1 && moveY == 0))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[2];
        }
        else if ((moveX == -1 && moveY == 1))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[3];
        }
        else if ((moveX == 0 && moveY == 1))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[4];
        }
        else if ((moveX == 1 && moveY == 1))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[5];
        }
        else if ((moveX == 1 && moveY == 0))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[6];
        }
        else if ((moveX == 1 && moveY == -1))
        {
            torsoSpriteRenderer.sprite = characterData.torsoSprites[7];
        }
    }
}
