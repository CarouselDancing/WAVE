using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Timers;
//using UnityEditor;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;
using Drawing;
using UnityEditor;
//using UnityEditor.Rendering;

public class CreatePath : MonoBehaviour
{
    public MusicPlayer music;
    public string performanceName;
    public int extraPointsBetweenBeats;
    //float tempo;
    public Transform handR_l, handR_r, handL_l, handL_r, head_l, head_r;
    Performance collection;
    string path;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
        path =  Application.persistentDataPath;
#endif
#if UNITY_EDITOR
        path = Application.dataPath;
#endif
        music.MultiplyTempo( 1 + extraPointsBetweenBeats);

        if(collection == null)
        {
            collection = new Performance();
            collection.Lhand = new List<Performance.Part>();
            collection.Rhand = new List<Performance.Part>();
            collection.head = new List<Performance.Part>();
        }
     

        points = new List<Vector3>[6];
        makingPath = new bool[3];
        timers = new int[3];

        transforms = new Transform[6];
        transforms[0] = handR_r;
        transforms[1] = handR_l;
        transforms[2] = handL_r;
        transforms[3] = handL_l;
        transforms[4] = head_r;
        transforms[5] = head_l;

       

        collection.name = performanceName;
        collection.extraPointsBetweenBeats = extraPointsBetweenBeats;
       
    }

    //float timer = 0;
    //float beatTimer;
    List<Vector3>[] points; // Rr,Rl,Lr,Ll,H
    Transform[] transforms;
    bool[] makingPath; // R,L,H
    int[] timers;


    //bool onBeat;
    bool recording;
    void Update()
    {

        if (recording)
        {
            //onBeat = beatTimer >= tempo;
            //if (onBeat)
            //    beatTimer = 0;
            //beatTimer += Time.deltaTime;
            //print(onBeat);
            if (OVRInput.GetDown(OVRInput.RawButton.A)) //RIGHT
            {
                print("PRESS A");
                if (makingPath[0])
                    stopMakingPath(0);
                else
                    startMakingAPath(0);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.X)) //LEFT
            {
                print("PRESS X");

                if (makingPath[1])
                    stopMakingPath(1);
                else
                    startMakingAPath(1);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.Y)) //HEAD
            {
                print("PRESS Y");

                if (makingPath[2])
                    stopMakingPath(2);
                else
                    startMakingAPath(2);
            }
        }
       

        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick)) //STOP
        {
            if (!recording)
            {
                recording = true;
                startTime = music.source.time;
                collection.startTime = startTime;
                StartCoroutine(notMakingPath(0));
                StartCoroutine(notMakingPath(1));
                StartCoroutine(notMakingPath(2));
            }
            else
            {
                print("STOP PERFORMANCE");
                string jsonDataString = JsonUtility.ToJson(collection);
                File.WriteAllText(path + "/performance.json", jsonDataString);
                recording = false;
            }
          
        }


     

        //timer += Time.deltaTime;
        drawPerformance();
    }

    public void startMakingAPath(int index)
    {
        print("start " + index);
        makingPath[index] = true;
        points[index*2] = new List<Vector3>();
        points[index*2+1] = new List<Vector3>();
        StartCoroutine(makePath(index));
    }

    IEnumerator makePath(int index)
    {
        while (makingPath[index])
        {
            if (music.OnBeat()) 
            {
                print("HERE");
                var v = transforms[index*2].position;
                var v1 = transforms[index*2 + 1].position;
                points[index*2].Add(v);
                points[index*2 + 1].Add(v1);

                var g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.localScale *= 0.01f; 
                g.transform.position = v;

                var g1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g1.transform.localScale *= 0.01f;
                g1.transform.position = v1;
            }

            yield return null;
        }
       
    }

    IEnumerator notMakingPath(int index)
    {
        while (!makingPath[index])
        {
            if (music.OnBeat())
                timers[index] += 1;//Time.deltaTime;
            yield return null;
        }

       
    }

    public void stopMakingPath(int index)
    {
        print("stop " + index);

        Performance.Part part = new Performance.Part();
        part.PointsR = points[2*index];
        part.PointsL = points[2*index + 1];
        part.waitTime = timers[index];
        part.name = "Move";
        switch (index)
        {
            case 0:
                collection.Rhand.Add(part);
                break;
            case 1:
                collection.Lhand.Add(part);
                break;
            case 2:
                collection.head.Add(part);
                break;
            default:
                break;
        }

        timers[index] = 0;
        makingPath[index] = false;

        StartCoroutine(notMakingPath(index));
    }

    void drawPerformance()
    {
        void drawList(List<Vector3> l,Color c)
        {
            if (l == null || l.Count == 0)
                return;
            Draw.ingame.WireSphere(l[0], 0.01f, c);
            Draw.ingame.CatmullRom(l, c);
            for (int i = 1; i < l.Count; i++)
            {
                Draw.ingame.WireSphere(l[i], 0.01f, c);
                //Draw.ingame.Line(l[i], l[i - 1], c);
            }
        }
        foreach(var l in collection.Rhand)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
        foreach (var l in collection.Lhand)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
        foreach (var l in collection.head)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
    }
    
}
