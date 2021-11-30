using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThiefAudioManager : NetworkBehaviour
{
    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioClip[] musicClips;
    public AudioClip[] effectClips;

    private GameManager gm;
    private ThiefStatistics stats;

    public override void OnStartAuthority()
    {
        enabled = true;
        gm = FindObjectOfType<GameManager>();
        stats = GetComponent<ThiefStatistics>();
        base.OnStartAuthority();
    }

    private void Update()
    {
        if (stats.detectValue >= 1)
        {
            PlayAlert();
            PlayDetect();
        }

        if (stats.detectValue <= 0)
        {
            PlayStealth();
        }
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
        gm.CmdPlayAlert();
    }

    
}
