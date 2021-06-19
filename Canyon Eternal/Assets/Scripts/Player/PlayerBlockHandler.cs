using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHandler : MonoBehaviour
{
    PlayerAnimatorHandler playerAnimatorHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;
    public BlockCollider blockCollider;

    public GameObject shieldModel;

    float blockDuration = 0.35f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        blockCollider = GetComponentInChildren<BlockCollider>();
    }

    public IEnumerator HandleBlocking()
    {
        playerManager.isBlocking = true;
        playerStats.LoseStamina(1);

        if (playerManager.isInteracting)
        {
            yield break;
        }
        else
        {
            playerAnimatorHandler.PlayTargetAnimation("Block", true);
            SFXPlayer.Instance.PlaySFXAudioClip(playerStats.characterData.blockMissed);

            yield return new WaitForSeconds(0.1f);

            shieldModel.SetActive(true);
        }
        yield return new WaitForSeconds(blockDuration);

        shieldModel.SetActive(false);
        playerManager.isBlocking = false;
    }

}
