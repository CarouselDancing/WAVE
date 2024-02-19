using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnBehaviour : PlayableBehaviour
{
    public float endTime;
    public GameObject prefab;
    public int index;
    public bool destroy;
    public bool isLevelChild;
    int counter;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        counter++;
        if (counter == 1 && Application.isPlaying)
        {
            var lib = playerData as Spawner;
            lib.SpawnPrefab(endTime, prefab,index,destroy, isLevelChild);
            base.ProcessFrame(playable, info, playerData);
        }

    }
}
