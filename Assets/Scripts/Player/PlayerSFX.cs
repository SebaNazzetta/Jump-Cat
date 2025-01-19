using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _jumpSoundFX;
    [SerializeField] private AudioClip _landSoundFX;
    [SerializeField] private AudioClip _wallHitSoundFX;
    [SerializeField] private AudioClip _hurtSoundFX;
    private SpriteRenderer _spriteRenderer;

    private void Awake() 
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void PlayJumpSoundFX()
    {
        if (!_spriteRenderer.enabled) return;
        SoundFXManager.instance.PlaySoundFXClip(_jumpSoundFX, transform, 1f);
    }

    private void PlayLandSoundFX()
    {
        if (!_spriteRenderer.enabled) return;
        SoundFXManager.instance.PlaySoundFXClip(_landSoundFX, transform, 1f);
    }

    private void PlayWallHitSoundFX()
    {
        if (!_spriteRenderer.enabled) return;
        SoundFXManager.instance.PlaySoundFXClip(_wallHitSoundFX, transform, 1f);
    }

    private void PlayHurtSoundFX()
    {
        if (!_spriteRenderer.enabled) return;
        SoundFXManager.instance.PlaySoundFXClip(_hurtSoundFX, transform, 1f);
    }
}
