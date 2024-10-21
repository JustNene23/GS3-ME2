using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioClip firstSong;
    public AudioClip secondSong;

    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayFirstSong();
    }

    private void PlayFirstSong()
    {
        audioSource.clip = firstSong;
        audioSource.Play();
        Invoke("PlaySecondSong", firstSong.length);
    }

    private void PlaySecondSong()
    {
        audioSource.clip = secondSong;
        audioSource.Play();
        Invoke("PlayFirstSong" , secondSong.length);
    }
}
