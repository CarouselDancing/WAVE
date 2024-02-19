using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(DanceLine))]
[TrackClipType(typeof(DanceStringClip))]
public class DanceStringTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        //return ScriptPlayable<DanceStringMixer>.Create(graph, inputCount);
        var clips = GetClips().ToList();
        
        for (int i = 0; i < clips.Count; i++)
        {
            var clip = clips[i];
           

            var myAsset = clip.asset as DanceStringClip;
            if (myAsset)
            {
                //myAsset.MakeString((float)clip.start, (float)clip.end, "");
                myAsset.start = (float)clip.start;
                myAsset.end = myAsset.OneBeat ? (float)clip.start : (float)clip.end;
                myAsset.nextStart = i < clips.Count - 1 ? (float)clips[i + 1].start : (float)clip.end;
            }
        }
        
        return base.CreateTrackMixer(graph, go, inputCount);
    }
}
