using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Threading;


public class CameraShake : MonoBehaviour
{
    public static CameraShake instance; // Singleton instance

    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;
    private CinemachineBasicMultiChannelPerlin perlin;
    private void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;

        shakeTimer = time;
        
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
        shakeTimer = 0f;
    }

    private void Update()
    {
        if(shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            if(shakeTimer <= 0f)
            {
                StopShake();
            }
        }
    }



}