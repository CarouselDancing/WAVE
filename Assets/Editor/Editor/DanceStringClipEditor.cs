using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Timeline;
using UnityEditor.Timeline;

[CustomTimelineEditor(typeof(DanceStringClip))]
public class TransformTweenClipEditor : ClipEditor
{
    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        var clipOptions = base.GetClipOptions(clip);
        clipOptions.highlightColor = Color.white;
        var a =  clip.asset as DanceStringClip;
        if (a.start == a.end)
            clipOptions.highlightColor = Color.magenta;
        if(a.skip)
            clipOptions.highlightColor = Color.gray;
        clip.displayName = a.style == "" ? "basic" : a.style;
        return clipOptions;
    }
}
