using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; }
    private CinemachineVirtualCamera cinemachine;
    private CinemachineBasicMultiChannelPerlin cmp;
    private float startintensity;
    private float shakeTime;
    private float startTime;

    private void Awake()
    {
        instance = this;
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        cmp = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    public void SetUpShake(float intensity, float duration)
    {
        cmp.m_AmplitudeGain = intensity;
        startintensity = intensity;
        shakeTime = duration;
        startTime = duration;
    }

    private void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
            cmp.m_AmplitudeGain = Mathf.Lerp(startintensity, 0f, 1 - startTime / shakeTime);
        }
    }
}
