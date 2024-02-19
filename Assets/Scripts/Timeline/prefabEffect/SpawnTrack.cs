using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Spawner))]
[TrackClipType(typeof(SpawnClip))]
public class SpawnTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        //return ScriptPlayable<DanceStringMixer>.Create(graph, inputCount);
        var clips = GetClips().ToList();

        for (int i = 0; i < clips.Count; i++)
        {
            var clip = clips[i];


            var myAsset = clip.asset as SpawnClip;
            if (myAsset)
            {
                myAsset.endTime = (float)clip.end;
            }
        }

        return base.CreateTrackMixer(graph, go, inputCount);
    }
}
