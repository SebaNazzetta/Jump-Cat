using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource _soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //Spawn in GameObject
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip;

        // Assign the volume
        audioSource.volume = volume;

        //Play the sound
        audioSource.Play();

        //Get length of sound FX clip
        float clipLength = audioClip.length;

        //Destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
