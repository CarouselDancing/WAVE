using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource source;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void _PlaySound(AudioClip clip)
    {
        if (timer > 0.2f || clip.name != source.clip.name || !source.isPlaying)
        {
            timer = 0;
            source.clip = clip;
            source.Play();
        }
        
    }
}
