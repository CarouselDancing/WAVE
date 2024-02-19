using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnClip : PlayableAsset
{
    public GameObject prefab;
    public int ObjectIndex = -1;
    public bool doNotDestroy;
    public bool isLevelChild = true;
    [HideInInspector] public float endTime;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpawnBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.endTime = endTime;
        behaviour.prefab = prefab;
        behaviour.index = ObjectIndex;
        behaviour.destroy = !doNotDestroy;
        behaviour.isLevelChild = isLevelChild;
        //behaviour.style = style;
        //behaviour.start = start;
        //behaviour.end = end;
        //behaviour.nextStart = nextStart;


        return playable;
    }
}
