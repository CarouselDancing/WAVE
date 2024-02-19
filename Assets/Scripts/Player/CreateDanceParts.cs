using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateDanceParts : MonoBehaviour
{
    public GameObject visualization;
    BeatPlayer beat;
    public string Name;
    [Range(1,10)] public int tempoMultiply;
    float tempo;
    int beatCount;
    // Start is called before the first frame update
    void Start()
    {
        beat = GameObject.FindObjectOfType<BeatPlayer>();
        tempo = 60f / beat.track.BPM / tempoMultiply;

#if UNITY_EDITOR
        if (danceParts == null)
        {
            danceParts = new importantDanceParts();

            if (Name == "")
                Name = "danceParts";
            AssetDatabase.CreateAsset(danceParts, "Assets/Resources/Performances/" + Name + ".asset");

            danceParts.importantParts = new List<importantDanceParts.DancePart>();
            danceParts.tempoMultiply = tempoMultiply;

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(danceParts);
            AssetDatabase.SaveAssets();
        }

#endif   

        visualization.SetActive(false);

    }

    public importantDanceParts danceParts;
    importantDanceParts.DancePart dancePart;

    bool recording;
    // Update is called once per frame
    void Update()
    {
        if (tempo <= beat.AudioSourceTime() - beatCount * tempo)
        {
            beatCount++;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            dancePart = new importantDanceParts.DancePart();
            float missAmount = (beat.AudioSourceTime() - beatCount * tempo) - (tempo / 2f);
            if (missAmount < 0)
                dancePart.startBeat = beatCount;
            else
                dancePart.startBeat = beatCount + 1;

            visualization.SetActive(true);
            recording = true;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            float missAmount = (beat.AudioSourceTime() - beatCount * tempo) - (tempo / 2f);
            if (missAmount < 0)
                dancePart.endBeat = beatCount;
            else
                dancePart.endBeat = beatCount + 1;
            int i = 0;
            bool found = false;
            for ( ; i < danceParts.importantParts.Count; i++)
            {
                if(danceParts.importantParts[i].startBeat > dancePart.startBeat)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                danceParts.importantParts.Insert(i, dancePart);
                while (i+1 < danceParts.importantParts.Count &&  
                    danceParts.importantParts[i+1].startBeat < dancePart.endBeat)
                {
                    if (danceParts.importantParts[i+1].endBeat < dancePart.endBeat)
                        danceParts.importantParts.RemoveAt(i);
                    else
                    {
                        var d = new importantDanceParts.DancePart();
                        d.startBeat = danceParts.importantParts[i].startBeat;
                        d.endBeat = danceParts.importantParts[i+1].endBeat;
                        danceParts.importantParts[i] = d;
                        danceParts.importantParts.RemoveAt(i);
                    }
                }
            }
            else
                danceParts.importantParts.Add(dancePart);


            AssetDatabase.Refresh();
            EditorUtility.SetDirty(danceParts);
            AssetDatabase.SaveAssets();

            recording = false;
            visualization.SetActive(false);

        }
#endif
        if (Input.GetKeyDown(KeyCode.I))
        {

        }

    }

 
}
