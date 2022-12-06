using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static void PlayOneShot(AudioClip audioClip, GameObject audioObject) 
    {
        GameObject audioGameObject = new GameObject("Audio");
        audioGameObject.transform.SetParent(audioObject.transform);

        AudioSource audioSource = audioGameObject.AddComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.PlayOneShot(audioClip);
    }
}
