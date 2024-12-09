using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _jumpSoundFX;
    [SerializeField] private AudioClip _landSoundFX;
    [SerializeField] private AudioClip _wallHitSoundFX;
    [SerializeField] private AudioClip _hurtSoundFX;


    public void PlayJumpSoundFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(_jumpSoundFX, transform, 1f);
    }
    public void PlayLandSoundFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(_landSoundFX, transform, 1f);
    }

    public void PlayWallHitSoundFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(_wallHitSoundFX, transform, 1f);
    }

    public void PlayHurtSoundFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(_hurtSoundFX, transform, 1f);
    }
}
