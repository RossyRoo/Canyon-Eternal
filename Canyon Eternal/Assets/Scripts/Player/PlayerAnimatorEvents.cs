using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMeleeHandler playerMeleeHandler;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
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

    public void PlayWeaponSwingSFX()
    {
        playerManager.sFXPlayer.PlaySFXAudioClip(playerMeleeHandler.activeMeleeCard.meleeWeaponSFXBank.swingWeapon);
    }
}
