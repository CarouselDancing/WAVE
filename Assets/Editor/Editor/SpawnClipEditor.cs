using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(SpawnClip))]
public class SpawnClipEditor : ClipEditor
{
    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        var clipOptions = base.GetClipOptions(clip);
       
        var a = clip.asset as SpawnClip;
        if (a.doNotDestroy)
            clipOptions.highlightColor = Color.magenta;
        else
            clipOptions.highlightColor = Color.cyan;

        if (a.prefab == null)
            clip.displayName = a.ObjectIndex + "";
        else
            clip.displayName = a.prefab.name;
        return clipOptions;
    }
}
