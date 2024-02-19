using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DanceStringBehaviour : PlayableBehaviour
{
    public string style;
    public float start, end, nextStart;
    int counter;
    public bool skip, oneBeat;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        counter++;
        if (counter == 1 && Application.isPlaying && !skip)
        {
            
            DanceLine line = playerData as DanceLine;
            line.MakeString(start, end,nextStart, style, oneBeat);
            base.ProcessFrame(playable, info, playerData);
        }
      
    }

    public override void OnGraphStart(Playable playable)
    {
        counter = 0;
        base.OnGraphStart(playable);
    }

}
