using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    PlayerMeleeHandler playerMeleeHandler;

    private void Awake()
    {
        playerMeleeHandler = GetComponentInParent<PlayerMeleeHandler>();
    }

    public void DespawnMelee()
    {
        playerMeleeHandler.currentMeleeModel.SetActive(false);
    }

    public void OpenDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        playerMeleeHandler.meleeDamageCollider.DisableDamageCollider();
    }
}
