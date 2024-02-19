using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelInfo : MonoBehaviour
{
    [TextArea] public string info;
    public Questionaire questionaire;
    public PlayableDirector director;
    public string animationName;
    [Space]
    public int beatsToTarget;
    public int BPM;
    [Range(1,10)] public int beatDivide;
    public bool useOffset; 
    public float offset;

    [Space]
    public SkinnedMeshRenderer teacherModel;
    public GameObject realTimePartners;
    public float eyeLevel;

    [Space]
    public Vector3 PlayerOffset;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(showBody());
    }

    IEnumerator showBody()
    {
        float t = 0;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            yield return null;
        }
        teacherModel.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
