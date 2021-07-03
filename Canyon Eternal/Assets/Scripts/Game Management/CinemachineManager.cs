using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance { get; private set; }

    [Header("VCams")]
    public CinemachineStateDrivenCamera stateDrivenCamera;
    public CinemachineVirtualCamera activeCM;
    public CinemachineVirtualCamera followCM;
    public CinemachineVirtualCamera combatCM;
    public CinemachineVirtualCamera dialogueCM;

    PlayerManager playerManager;
    Animator animator;

    float shakeTimer;


    private void Awake()
    {
        stateDrivenCamera = GetComponent<CinemachineStateDrivenCamera>();
        animator = GetComponent<Animator>();
    }

    public void OnLoadScene(PlayerManager _playerManager)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerManager = _playerManager;

        FindPlayer(playerManager);

        activeCM = followCM;
        activeCM.enabled = false;
        activeCM.enabled = true;
    }

    public void LosePlayer()
    {
        activeCM.Follow = null;
        activeCM.LookAt = null;
    }

    public void FindPlayer(PlayerManager playerManager)
    {
        stateDrivenCamera.Follow = playerManager.transform;
        stateDrivenCamera.LookAt = playerManager.transform;
    }

    public void SwitchCamerasByPlayerState()
    {
        if (playerManager.isInCombat || playerManager.isCastingSpell)
        {
            activeCM = combatCM;
            animator.Play("Combat State");
        }
        else if(!playerManager.isInCombat && !playerManager.isCastingSpell && !playerManager.isConversing)
        {
            activeCM = followCM;
            animator.Play("Follow State");
        }
        /*else if(playerManager.isConversing)
        {
            activeCM = dialogueCM;
            animator.Play("Dialogue State");
        }*/
    }


    #region Shake
    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            activeCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void ShakeTimer()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        if (shakeTimer <= 0)
        {

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                activeCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }

    #endregion


    private void Update()
    {
        ShakeTimer();
        SwitchCamerasByPlayerState();
    }


}
