using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBPM : MonoBehaviour
{
    public int BPM;
    public float BPM_f; 
    public AudioSource MusicSource;
    public AudioSource TestSource;
    float timeBefore;
    List<float> differences;
    public bool playTestSound;
    public float offSet;
    public int divide;
    // Start is called before the first frame update
    void Start()
    {
        differences = new List<float>();
    }
    float tempo;
    float timer;
    int counter;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeBefore != 0)
            {
                differences.Add(MusicSource.time - timeBefore );
            }
            timeBefore = MusicSource.time;
            float sum = 0;
            differences.ForEach(d => sum += d);
            BPM_f = 60 / (sum / differences.Count);
            BPM = BPM_f - (int)BPM_f >= 0.5f ? (int)BPM_f + 1 : (int)BPM_f;
            print(BPM);
            
        }
        tempo = 60f / BPM * divide;
        timer = offSet + MusicSource.time - (tempo * counter);
        if(timer >= tempo)
        {
            counter++;
            if (playTestSound)
                TestSource.Play();
        }
    }
}
