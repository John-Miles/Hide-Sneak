using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullPlayer : MonoBehaviour
{

    public AudioClip[] clips;
    public AudioSource source;
    public int minDelay = 3;
    public int maxDelay = 12;
    private int delay;

    void Awake()
    {
        StartCoroutine(AttemptPlay());
    }

    IEnumerator AttemptPlay()
    {
        while (true)
        {
            //Check if the Audio is already Playing
            if (!source.isPlaying)
            {
                //Pick a random Clip to Play
                source.clip = clips[Random.Range(0, clips.Length)];
                source.Play();
            }

            delay = Random.Range(minDelay, maxDelay);

            yield return new WaitForSeconds(delay);
        }
    }


}
