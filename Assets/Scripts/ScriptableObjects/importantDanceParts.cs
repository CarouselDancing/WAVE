using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "danceParts", menuName = "ScriptableObjects/DanceParts", order = 1)]
public class importantDanceParts : ScriptableObject
{
    public string info;
    [Space]
    public int tempoMultiply;
    public bool everythingImportant;
    public List<DancePart> importantParts;

    [Serializable]
    public struct DancePart
    {
        public int startBeat;
        public int endBeat;
        public string effectName;
    }

}
