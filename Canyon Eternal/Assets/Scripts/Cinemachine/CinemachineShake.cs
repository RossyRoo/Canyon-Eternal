using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;


    public void OnLoadScene()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        cinemachineVirtualCamera.enabled = false;
        cinemachineVirtualCamera.enabled = true;
    }

    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        if(shakeTimer <= 0)
        {

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }

    public void SwitchToFallCam()
    {
        cinemachineVirtualCamera.Follow = null;
        cinemachineVirtualCamera.LookAt = null;
        cinemachineVirtualCamera.transform.parent = null;
    }

    public void SwitchToPlayer(PlayerManager playerManager)
    {
        cinemachineVirtualCamera.Follow = playerManager.transform;
        cinemachineVirtualCamera.LookAt = playerManager.transform;
        cinemachineVirtualCamera.transform.parent = playerManager.transform;
    }
}
