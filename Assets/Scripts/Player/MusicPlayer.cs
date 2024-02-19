using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] Track track;
    public AudioSource source;
    public AudioSource metronome;
    public bool useBPM;
    [SerializeField] VisualEffect beat;
    public bool setTempo;
    private void Awake()
    {
        tempoTimes = new List<float>();
        //if (useBPM)
        //    track.tempo = 60 / track.BPM;
        //tempo = track.tempo;
        playMusic();
    }
    List<float> tempoTimes;
    float timer;
    float tempo;
    bool onBeat;
    float beatTimer;
    float procent;
    bool pause;
    int beatCount;
    float oldTrackTime;
    // Update is called once per frame
    void Update()
    {
      
        if (setTempo)
        {
            timer += source.time - oldTrackTime;
            oldTrackTime = source.time;

            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    tempoTimes.Add(timer);
            //    timer = 0;
            //    float sum = 0;
            //    for (int i = 1; i < tempoTimes.Count; i++)
            //    {
            //        sum += tempoTimes[i];
            //    }
            //    if (tempoTimes.Count > 1)
            //    {
            //        sum = sum / (tempoTimes.Count - 1);
            //        track.tempo = sum;
            //        track.offset = (source.time % sum);// - sum / 2;
            //        if (track.offset < 0)
            //            track.offset += sum;
            //    }
            //}
            //beat.SetFloat("beat", track.tempo);
            
        }
        print(source.time);
        //track.tempo = 60f / beatsPerMinute;
        //tempo = track.tempo;
        if (source.time > track.offset)
        {
            onBeat = beatTimer >= tempo;
            procent = beatTimer / tempo;
            if (onBeat)
            {
                beatTimer = 0;
                beatCount++;
                print("beats: " +  beatCount);
                metronome.Play();
            }
            if (!pause)
            {
                beatTimer += source.time - oldTrackTime;
                oldTrackTime = source.time;
            }
        }
    }
    public float Tempo() => tempo;

    public int BeatCount() => beatCount;

    public void ClearBeats() => beatCount = 0;
    public void MultiplyTempo(int multi)
    { 
       // tempo = track.tempo / multi;
    }
    public void DivideTempo(int div)
    {
       // tempo = track.tempo * div;
    }

    public bool OnBeat()
    {
        return onBeat;
    }

    public bool TogglePause()
    {
        pause = !pause;
        if (pause)
            source.Pause();
        else
            source.Play();
        return pause;
    }
    public float BeatProcent()
    {
        return procent;
    }

    public void playMusic()
    {
        source.clip = track.music;
        source.Play();
       
        StartCoroutine(waitOffset());
    }

    IEnumerator waitOffset()
    {
        beat.enabled = false;
        while (source.time < track.offset)
            yield return null;
        //beat.SetFloat("beat", track.tempo);
        beat.enabled = true;
    }
}
