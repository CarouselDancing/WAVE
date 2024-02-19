using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "performance", menuName = "ScriptableObjects/Performance", order = 1)]
public class PerformanceSobj : ScriptableObject
{
    //Performance performance;
    public string Name;
    public int extraPointsBetweenBeats;
    public float originalPlayerHeight;
    public float startTime;
    [Space]
    public List<Performance.Part> Lhand;
    public List<Performance.Part> Rhand;
    public List<Performance.Part> head;

    public Performance Unpack()
    {
        var p = new Performance();
        p.name = Name;
        p.extraPointsBetweenBeats = extraPointsBetweenBeats;
        p.originalPlayerHeight = originalPlayerHeight;
        p.Lhand = Lhand;
        p.Rhand = Rhand;
        p.head = head;
        p.startTime = startTime;
        return p;
    }
}
