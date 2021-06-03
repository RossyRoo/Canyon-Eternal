using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public CharacterManager myCharacter;
    Collider2D blockCollider;

    private void Awake()
    {
        myCharacter = GetComponentInParent<CharacterManager>();
        blockCollider = GetComponent<Collider2D>();
        blockCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager characterCollision = collision.gameObject.GetComponent<CharacterManager>();

        if (characterCollision != null && characterCollision.transform != myCharacter.transform)
        {
            if(characterCollision.isVulnerableToBlock)
            {
                characterCollision.isStunned = true;

                SFXPlayer.Instance.PlaySFXAudioClip(myCharacter.GetComponent<CharacterStats>().characterSFXBank.block);
            }
        }
    }
}
