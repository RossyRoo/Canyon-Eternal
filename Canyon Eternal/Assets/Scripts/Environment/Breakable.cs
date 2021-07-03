using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    Animator animator;
    bool isBroken;

    [Header("SFX")]
    public AudioClip bumpSFX;
    public AudioClip breakSFX;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerManager playerManager = collision.GetComponentInParent<PlayerManager>();

        if (!isBroken && playerManager != null && collision.gameObject.layer == 15)
        {
            animator.Play("Break");
            isBroken = true;
            SFXPlayer.Instance.PlaySFXAudioClip(breakSFX, 0.1f);
            Destroy(GetComponent<BoxCollider2D>());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager characterManager = collision.GetComponentInParent<PlayerManager>();

        if (!isBroken && characterManager != null && !characterManager.isAttacking)
        {
            animator.Play("Bump");
            SFXPlayer.Instance.PlaySFXAudioClip(bumpSFX, 0.02f);
        }
    }
}
