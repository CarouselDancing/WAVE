using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    SpawnLibrary library;
    BeatPlayer beatPlayer;

    // Start is called before the first frame update
    void Start()
    {
        library = GameObject.FindObjectOfType<SpawnLibrary>();
        beatPlayer = GameObject.FindObjectOfType<BeatPlayer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPrefab(float endTime, GameObject obj,int index, bool destroy, bool isLevelChild)
    {
        int endBeat = Mathf.RoundToInt((endTime - beatPlayer.AudioOffset()) / beatPlayer.tempo);
        StartCoroutine(_spawnPrefab(beatPlayer.beatCount, endBeat, obj,index,destroy, isLevelChild));
    }

    IEnumerator _spawnPrefab(int beat, int endBeat, GameObject obj, int index, bool destroy, bool isLevelChild)
    {
        int b = beatPlayer.realTime ? beat + beatPlayer.levelInfo.beatsToTarget : beat;
        int eB = beatPlayer.realTime ? endBeat + beatPlayer.levelInfo.beatsToTarget : endBeat;
        
        while (beatPlayer.beatCount == b)
            yield return null;

        GameObject g = null;

        if(obj != null)
        {
            if (isLevelChild)
                g = Instantiate(obj, beatPlayer.levelInfo.transform);
            else
                g = Instantiate(obj);
            var life = g.GetComponent<LifeTime>();
            if (life != null)
            {
                life.lifeTime = beatPlayer.tempo * (eB - b);
                life.count = destroy;
            }
        }

        if (index >= 0 && transform.childCount > index)
        {
            var o = transform.GetChild(index).gameObject;
            o.SetActive(true);
            var life = o.GetComponent<LifeTime>();
            if (life != null)
            {
                life.lifeTime = beatPlayer.tempo * (eB - b);
                life.count = destroy;
            }
        }
            

        if (!destroy)
            yield break;

        while (beatPlayer.beatCount != eB)
            yield return null;

        if(obj != null)
            Destroy(g);
        if (index >= 0 && transform.childCount > index)
            transform.GetChild(index).gameObject.SetActive(false);
    }
}
