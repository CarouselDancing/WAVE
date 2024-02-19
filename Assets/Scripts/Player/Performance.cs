using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
[Serializable]
public class Performance 
{
    public string name; //Time between Movement's points;
    public float originalPlayerHeight;
    public int extraPointsBetweenBeats;
    public float startTime;
    [Space]
    public List<Part> Lhand;
    public List<Part> Rhand;
    public List<Part> head;

    [Serializable] 
    public struct Part
    {
        public string name;
        public int waitTime; //BEATS
        public List<Vector3> PointsL;
        public List<Vector3> PointsR;
    }
}
