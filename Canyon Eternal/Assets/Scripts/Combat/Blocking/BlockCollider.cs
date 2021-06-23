using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public CharacterManager myCharacter;
    Collider2D blockCollider;
    public AudioClip blockCollisionSFX;

    public bool targetIsWithinRange;

    private void Awake()
    {
        myCharacter = GetComponentInParent<CharacterManager>();
        blockCollider = GetComponent<Collider2D>();
        blockCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetIsWithinRange = true;

        CharacterManager characterCollision = collision.gameObject.GetComponent<CharacterManager>();

        if (characterCollision != null && characterCollision.transform != myCharacter.transform)
        {
            if(characterCollision.isVulnerableToBlock)
            {
                characterCollision.isStunned = true;

                SFXPlayer.Instance.PlaySFXAudioClip(blockCollisionSFX, 0.05f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targetIsWithinRange = false;
    }
}
