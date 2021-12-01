using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThiefAudioManager : NetworkBehaviour
{
    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioClip[] musicClips;
    public AudioClip[] footsteps;
    
    public float footstepDelay;

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

    public IEnumerator Walking()
    {
        effectsSource.clip = footsteps[Random.Range(0, footsteps.Length)];
        effectsSource.Play();
        yield return new WaitForSeconds(footstepDelay);
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
        gm.CmdPlayDetect();
    }

    public void PlayStealth()
    {
        gm.CmdPlayStealth();
    }

    public void PlayAlert()
    {
        gm.CmdPlayAlert();
    }

    
}
