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

    public void SwitchStateCam()
    {
        if (playerManager.isInCombat)
        {
            activeCM = combatCM;
            animator.Play("Combat State");
        }
        else if (!playerManager.isInCombat)
        {
            activeCM = followCM;
            animator.Play("Follow State");
        }
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
        SwitchStateCam();
    }


}
