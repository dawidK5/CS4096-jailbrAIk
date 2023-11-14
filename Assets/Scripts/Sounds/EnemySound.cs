using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip audioClip;

    void footSound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
