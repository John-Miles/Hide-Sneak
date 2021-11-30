using System.Collections;
using System.Collections.Generic;
using Mirror;
using Team_Members.NidgyWidgy.Scripts;
using UnityEngine;

public class GuardAudioManager : NetworkBehaviour
{
    
    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioClip[] musicClips;
    public AudioClip[] effectClips;

    private Flashlight flashlight;

    public override void OnStartAuthority()
    {
        enabled = true;
        flashlight = GetComponent<Flashlight>();
        base.OnStartAuthority();
    }
    void Update()
    {
    }
    
    public void PlayVictory()
    {
        musicSource.Stop();
        musicSource.clip = musicClips[0];
        musicSource.Play();
    }
    
    public void PlayDefeat()
    {
        musicSource.Stop();
        musicSource.clip = musicClips[1];
        musicSource.Play();
    }
    
    
    public void PlayDetect()
    {
        if (musicSource.clip != musicClips[2])
        {
            musicSource.Stop();
            musicSource.clip = musicClips[2];
            musicSource.Play();
        }
    }

    public void PlayStealth()
    {
        if (musicSource.clip != musicClips[3])
        {
            musicSource.Stop();
            musicSource.clip = musicClips[3];
            musicSource.Play(); 
        }
        
    }

    public void PlayAlert()
    {
        if (!effectsSource.clip == effectClips[0])
        {
            effectsSource.clip = effectClips[0];
            effectsSource.Play();
        }
    }
}
