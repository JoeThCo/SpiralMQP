using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Play sound when Pool Manager activate the object 
    private void OnEnable()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    // Stop sound when Pool Manager disable the object 
    private void OnDisable()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// Set the sound effect to play 
    /// </summary>
    public void SetSound(SoundEffectSO soundEffect)
    {
        // Set random pitch 
        audioSource.pitch = Random.Range(soundEffect.soundEffectPitchRandomVariationMin, soundEffect.soundEffectPitchRandomVariationMax);
        audioSource.volume = soundEffect.soundEffectVolume; // Set volume
        audioSource.clip = soundEffect.soundEffectClip; // Set source clip
    }
}
