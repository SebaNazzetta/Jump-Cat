using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    private Camera _mainCamera;
    private CinemachineVirtualCamera _currentCamera;
    [SerializeField, Range(0.1f, 10f)] private float _shakeIntensity = 1.5f;
    [SerializeField, Range(0, 10f)] private float _timeShaking = 0.5f;
    private WaitForSeconds _shakeDuration;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _shakeDuration = new WaitForSeconds(_timeShaking/2);
    }
    private void GetCurrentCamera()
    {
        _currentCamera = _mainCamera.GetComponent<CinemachineBrain>()
            .ActiveVirtualCamera.VirtualCameraGameObject
            .GetComponent<CinemachineVirtualCamera>();

    }

    public void Shake()
    {
        GetCurrentCamera();
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        var currentCameraPerlin = _currentCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        currentCameraPerlin.m_AmplitudeGain = _shakeIntensity;

        yield return _shakeDuration;
        
        var totalTime = _timeShaking/2;
        var currentTime = 0f;
        while(currentTime < totalTime)
        {
            currentCameraPerlin.m_AmplitudeGain = 
                Mathf.Lerp(_shakeIntensity, 0, currentTime/totalTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentCameraPerlin.m_AmplitudeGain = 0;
    }
}
