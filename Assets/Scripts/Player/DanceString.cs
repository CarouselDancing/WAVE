using DG.Tweening;
using Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceString : MonoBehaviour
{
    public PointTarget goalIndicator;
    //public LineRenderer lineRenderer;
    public GameObject line1;
    public GameObject line2;
    public GameObject stringStartPoint;

    public bool useBeatPlayersBeatsToTarget = true;
    public importantDanceParts danceParts;
    public Transform parent;
    public Transform target;
    public float beatsToTarget;
    public int stringResolution;
    BeatPlayer beat;
    Queue<Vector3> points;

    public float tempo;
    public int beatCount;
    public int previewLenght;
    bool makingString;

    Queue<Vector3> indicatorPos;
    // Start is called before the first frame update
    void Start()
    {
        indicatorPos = new Queue<Vector3>();
        beat = GameObject.FindObjectOfType<BeatPlayer>();
        tempo = 60f/ beat.track.BPM / (danceParts.tempoMultiply);
        points = new Queue<Vector3>();

        if (useBeatPlayersBeatsToTarget)
            beatsToTarget = beat.beatsToTarget;

        goalIndicator.neverTouched = true;
        //StartCoroutine(MoveIndicator());
    }

    int index;
    bool started;
    int next_;
    // Update is called once per frame
    void Update()
    {
        //addIndicatorPoint();
        //if(beat.source.isPlaying && started)
        //{
        //    started = false;
        //    HideUntilNext();
        //}
      

        if (tempo <= beat.AudioSourceTime() - beatCount * tempo)
        {
            beatCount++;
        }

        if (danceParts.everythingImportant)
            makingString = true;
        else if (index < danceParts.importantParts.Count)
        {
            if(beatCount == danceParts.importantParts[index].startBeat)
            {
                print("HI");
                StartCoroutine(StringMaker(index));
                index++;
            }
            //if (beatCount >= danceParts.importantParts[index].startBeat)
            //    makingString = true;
            //if (beatCount >= danceParts.importantParts[index].endBeat)
            //{
            //    makingString = false;
            //    if (index < danceParts.importantParts.Count)
            //        index++;
            //}
        }

    }

    void HideUntilNext(Vector3 goal, int next)
    {
        //return;

        //if (index == danceParts.importantParts.Count)
        //    return;

        var part = danceParts.importantParts[next];

        float t = part.startBeat * beat.tempo + beatsToTarget  * tempo - (beat.AudioSourceTime()); //(beatCount * tempo + (beat.source.time - beatCount * tempo));
        //print("HI " + t + " " + part.startBeat + " " + beatsToTarget + " " + danceParts.tempoMultiply + " " + tempo + " " + beat.source.time);
        goalIndicator.transform.DOKill();
        goalIndicator.transform.DOMove(goal, t);
        goalIndicator.HideAndShow(t);
    }

    void addIndicatorPoint()
    {
        var M1 = parent.worldToLocalMatrix;
        var M2 = target.localToWorldMatrix;
        indicatorPos.Enqueue (M2.MultiplyPoint3x4(M1.MultiplyPoint3x4(transform.position)));
    }
    IEnumerator MoveIndicator()
    {
        yield return new WaitForSeconds(beat.tempo * beat.beatsToTarget);
        while(true)
        {
            goalIndicator.transform.position = indicatorPos.Dequeue();
            yield return null;
        }
    }

    int stringsBeingDrawn;
    IEnumerator StringMaker(int DancePartListIndex)
    {
        //goalIndicator.transform.DOKill();
        stringsBeingDrawn++;
        var startPoint = Instantiate( stringStartPoint);
        var line = Instantiate(line1).GetComponent<LineRenderer>();
        var line_ = Instantiate(line2).GetComponent<LineRenderer>();

        GameObject effect = Instantiate( beat.styleLib.styles[ danceParts.importantParts[DancePartListIndex].effectName],goalIndicator.transform);

        bool neverDestroyed = true;
        bool first = true;
        goalIndicator.neverTouched = true;

        int endBeat = danceParts.importantParts[DancePartListIndex].endBeat;
        int destroyed = 0;

        Matrix4x4 M1, M2;
        List<Vector3> danceString;
        List<Vector3> startPoints;
        List<Vector3> endPoints;
        List<float> lifeTimes;

        startPoints = new List<Vector3>();
        endPoints = new List<Vector3>();
        lifeTimes = new List<float>();
        danceString = new List<Vector3>();

        float resolution = beat.tempo / stringResolution;
        float timer = resolution;

        int previewNumber = (int)((previewLenght * beat.tempo) / resolution);
        void UpdateStringVisuals()
        {
            line.positionCount = Mathf.Min(endPoints.Count, previewNumber);
            line.SetPositions(endPoints.GetRange(0,Mathf.Min(endPoints.Count, previewNumber)).ToArray());
            line_.positionCount = danceString.Count;
            line_.SetPositions(danceString.ToArray());
        }

        void UpdateStartPoint()
        {
            if (startPoints.Count > 0 && destroyed == 0)
            {
                startPoint.transform.position = danceString[0];
            }
            else if (destroyed == 1)
                startPoint.transform.DOScale(0, 0.1f);
        }

        do
        {
            if (timer >= resolution)
            {
                timer = 0;
               

                startPoints.Add(transform.position);
                danceString.Add(transform.position);
                lifeTimes.Add(0);
                M1 = parent.worldToLocalMatrix;
                M2 = target.localToWorldMatrix;
                endPoints.Add(M2.MultiplyPoint3x4(M1.MultiplyPoint3x4(transform.position)));

                UpdateStringVisuals();

                if (first)
                {
                    first = false;
                    

                    if (stringsBeingDrawn == 1)// points.Count == 0)
                    {
                        print("JUPJPUPJPUP");
                        HideUntilNext(endPoints[0], DancePartListIndex);
                    }
                    else
                        points.Enqueue(endPoints[0]);

                }
            }

            LerpAll(transform.position);
            //Draw.ingame.CatmullRom(danceString);

            timer += Time.deltaTime;
            yield return null;
        }
        while (beatCount < endBeat);


        startPoints.Add(transform.position);
        danceString.Add(transform.position);
        lifeTimes.Add(0);
        M1 = parent.worldToLocalMatrix;
        M2 = target.localToWorldMatrix;
        endPoints.Add(M2.MultiplyPoint3x4(M1.MultiplyPoint3x4(transform.position)));

        UpdateStringVisuals();



        while (Mathf.Min(1, (lifeTimes[lifeTimes.Count-1] / resolution) / beatsToTarget / stringResolution) != 1)
        {
            LerpAll(danceString[lifeTimes.Count - 1]);

            yield return null;
        }

        void LerpAll(Vector3 firstPosition)
        {
            float lerpProcent;
            for (int i = lifeTimes.Count - 1; i > -1; i--)
            {
                lifeTimes[i] += Time.deltaTime;
                //if (lerpProcent == 1)
                //    lifeTimes[i] = (resolution * beatsToTarget * stringResolution);
                
            }
            Vector3 previous = firstPosition;
            for (int i = lifeTimes.Count - 1; i > -1; i--)
            {
                //lifeTimes[i] += Time.deltaTime;
                //float lerpProcent = Mathf.Min(1, lifeTimes[i] / resolution / beatsToTarget / stringResolution);
                //if (lerpProcent == 1)
                //    lifeTimes[i] = (resolution * beatsToTarget * stringResolution);
                lerpProcent = Mathf.Min(1, lifeTimes[i] / resolution / beatsToTarget / stringResolution);
                danceString[i] = Vector3.Lerp(startPoints[i], endPoints[i], lerpProcent);
                //if (neverDestroyed && i == 0)
                //    Draw.ingame.WireSphere(danceString[i], 0.02f, Color.red);
                if (lerpProcent == 1)
                {
                    Vector3 goal;
                    if (lifeTimes.Count > i + 1)
                    {
                        lerpProcent = Mathf.Min(1, 1 -( (resolution * beatsToTarget * stringResolution - lifeTimes[i + 1]) / resolution));// Mathf.Min(1, (lifeTimes[i + 1] / resolution) / beatsToTarget / stringResolution);
                        goal = endPoints[i] + (endPoints[i + 1] - endPoints[i]) * lerpProcent;
                    }
                    else
                    {
                        goal = danceString[i];
                    }
                    goalIndicator.transform.position = goal;
                    //Draw.ingame.Line(previous, goal);
                    //if ((i+destroyed) % stringResolution == 0)
                    //    Draw.ingame.WireSphere(goal, 0.1f);
                   
                    i--;
                    while (i > -1)
                    {
                        neverDestroyed = false;
                        lifeTimes.RemoveAt(0);
                        startPoints.RemoveAt(0);
                        endPoints.RemoveAt(0);
                        danceString.RemoveAt(0);
                        destroyed++;
                        i--;
                    }
                    break;

                    
                }
                //Draw.ingame.Line(previous, danceString[i]);
                //Draw.ingame.WireSphere(endPoints[i], 0.01f / i, Color.black);
                //if ((i+destroyed) % stringResolution == 0)
                //    Draw.ingame.WireSphere(danceString[i], 0.02f,Color.gray);
                previous = danceString[i];

            }

            for(int i = 0; i < endPoints.Count; i++)
            {
                //Draw.ingame.WireSphere(endPoints[i], 0.001f, Color.black);
            }
           
            //lineRenderer.
            if (goalIndicator.active)
            {
                if (goalIndicator.neverTouched)
                {
                    print("START");
                    effect.GetComponent<stringVariables>().StartEvent(goalIndicator);
                }
                else
                {
                    effect.GetComponent<stringVariables>().DuringEvent();
                }
            }

            UpdateStringVisuals();
            UpdateStartPoint();

        }

        if (goalIndicator.active)
        {
            print("END");

            effect.GetComponent<stringVariables>().EndEvent(goalIndicator);
        }
        print(points.Count);
        
       
        //if (points.Count > 0)
        //    points.Dequeue();
        //goalIndicator.visuals.transform.DOScale(0, 0.1f);
        Destroy(line.gameObject);
        Destroy(line_.gameObject);
        Destroy(startPoint);
        stringsBeingDrawn--; ;
        if (stringsBeingDrawn > 0)
            HideUntilNext(points.Dequeue(), DancePartListIndex + 1);
        else
            goalIndicator.visuals.transform.DOScale(0f, 0.1f);
    }
}
