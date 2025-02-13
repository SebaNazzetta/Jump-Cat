using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private CameraManager _cameraManager;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ShakeCamera()
    {
        if (!_spriteRenderer.enabled) return;
        _cameraManager.Shake();
        FindObjectOfType<SettingsManager>().DefaultVibration();
    }


}
