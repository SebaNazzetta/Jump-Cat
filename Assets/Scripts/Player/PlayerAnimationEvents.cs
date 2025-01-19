using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private CameraManager _cameraManager;
    private void Awake()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void ShakeCamera()
    {
        _cameraManager.Shake();
    }
}
