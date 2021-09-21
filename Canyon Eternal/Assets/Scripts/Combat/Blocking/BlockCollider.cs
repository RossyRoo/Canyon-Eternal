using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public OffhandWeapon offhandWeaponData;

    public AudioClip blockCollisionSFX;

    public bool parryMode;
    public bool blockMode;
    public bool canBlockEnemy = true;
    public bool canBlockPlayer = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(parryMode)
        {
            CharacterManager characterCollision = collision.gameObject.GetComponent<CharacterManager>();

            if (characterCollision != null)
            {
                if (characterCollision.GetType() == typeof(EnemyManager) && canBlockEnemy
                    || characterCollision.GetType() == typeof(PlayerManager) && canBlockPlayer)
                {

                    if (characterCollision.isVulnerableToParry)
                    {
                        characterCollision.isStunned = true;

                        SFXPlayer.Instance.PlaySFXAudioClip(blockCollisionSFX, 0.05f);
                    }
                }
            }
        }
        else if(blockMode)
        {
            DamageCollider damageColliderCollision = collision.gameObject.GetComponent<DamageCollider>();

            if (damageColliderCollision != null)
            {
                StartCoroutine(damageColliderCollision.TemporarilyDisableDamage(0.5f));
            }
        }

    }



}
