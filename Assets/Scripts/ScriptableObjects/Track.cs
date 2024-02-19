using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "track", menuName = "ScriptableObjects/Track", order = 1)]
public class Track : ScriptableObject
{
    public AudioClip music;
    public float offset;
    public string animationName;
    public float BPM;
    [Range(1,10)] public int beatDivide;
    //public float tempo;
}
