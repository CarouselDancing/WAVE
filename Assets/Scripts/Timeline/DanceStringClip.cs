using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DanceStringClip : PlayableAsset
{
    public string style;
    [Space]
    
    public float start, end, nextStart;
    public bool OneBeat,skip;
    int counter;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DanceStringBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.style = style;
        behaviour.start = start;
        behaviour.end = end;
        behaviour.nextStart = nextStart;
        behaviour.skip = skip;
        behaviour.oneBeat = OneBeat;
        Debug.Log("Make behaviour");
       
        return playable;
    }
}
