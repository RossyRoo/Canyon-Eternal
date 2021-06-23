using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;

    public GameObject shieldModel;
    public AudioClip blockMissedSFX;

    float blockDuration = 0.35f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
    }

    public IEnumerator HandleBlocking()
    {
        playerManager.isBlocking = true;

        playerStats.LoseStamina(1);
        playerStats.EnableInvulnerability(playerStats.characterData.invulnerabilityFrames);

        playerAnimatorHandler.PlayTargetAnimation("Block", true);
        SFXPlayer.Instance.PlaySFXAudioClip(blockMissedSFX, 0.3f, 0.2f);

        shieldModel.SetActive(true);

        yield return new WaitForSeconds(blockDuration);

        shieldModel.SetActive(false);
        playerManager.isBlocking = false;
    }

}
