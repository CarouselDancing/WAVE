using DG.Tweening;
using Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DanceLine : MonoBehaviour
{
    public string movementCopyName;

    [Space]
    public GameObject pointTarget;
    public Shader targetShader;
    public string targetName;
    public Color normalColor;
    public Color activeColor;
    public GameObject arrow;
    [Space]
    PointTarget goalIndicator;

    public GameObject targetLine;

    public GameObject stringStartPoint;

    public bool useBeatPlayersBeatsToTarget = true;
    //public importantDanceParts danceParts;
    Transform parent;
    Transform target;
    public float beatsToTarget;
    public int stringResolution;
    BeatPlayer beat;
    Queue<Vector3> points;

    float tempo;
    public int beatCount;
    public int doupleBeatCount;

    public int previewLenght;
    bool makingString;

    Queue<Vector3> indicatorPos;


    // Start is called before the first frame update
    void Start()
    {
        

        goalIndicator = Instantiate(pointTarget,null).GetComponent<PointTarget>();
        goalIndicator.targetName = targetName;
        goalIndicator.visuals.transform.localScale = Vector3.zero;

        Material M = new Material(targetShader);;
        M.CopyPropertiesFromMaterial(goalIndicator.normal);
        M.SetColor("Color_17ec2ad908114c7a9b85b2ae24cdc302", normalColor);
        goalIndicator.normal = M;

        M = new Material(targetShader); ;
        M.CopyPropertiesFromMaterial(goalIndicator.activated);
        M.SetColor("Color_17ec2ad908114c7a9b85b2ae24cdc302", activeColor);
        goalIndicator.activated = M;


        var a = Instantiate(arrow, GameObject.Find(targetName).transform);
        a.GetComponent<LookAt>().target = goalIndicator.transform;
        a.transform.GetChild(0).GetComponent<LineRenderer>().material = M;

        goalIndicator.arrowScale = a.transform.localScale;
        goalIndicator.arrow = a.transform;


        indicatorPos = new Queue<Vector3>();
        beat = GameObject.FindObjectOfType<BeatPlayer>();
        tempo = beat.tempo;
        points = new Queue<Vector3>();


        //if (beat.realTime)
        //    findTarget(transform);
        //else
            findTarget(beat.levelInfo.teacherModel.transform.parent);


        if (target == null)
            target = beat.Player;
        if (parent == null)
            parent = transform.parent.parent;

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
        

        if (stringsBeingDrawn == 0)
            goalIndicator.visuals.transform.DOScale(0, 0.1f);// = Vector3.zero;

        if (tempo <= beat.AudioSourceTime() - beatCount * tempo)
        {
            beatCount++;
        }

        if (tempo/2 <= beat.AudioSourceTime() - doupleBeatCount * tempo/2)
        {
            doupleBeatCount++;
        }

        //else if (index < danceParts.importantParts.Count)
        //{
        //    if (beatCount == danceParts.importantParts[index].startBeat)
        //    {
        //        print("HI");
        //        StartCoroutine(StringMaker(index));
        //        index++;
        //    }
        //    //if (beatCount >= danceParts.importantParts[index].startBeat)
        //    //    makingString = true;
        //    //if (beatCount >= danceParts.importantParts[index].endBeat)
        //    //{
        //    //    makingString = false;
        //    //    if (index < danceParts.importantParts.Count)
        //    //        index++;
        //    //}
        //}

    }

    public void findTarget(Transform TargetParent)
    {
        void lookForTarget(Transform t)
        {
            foreach (Transform child in t)
            {

                if (child.name == movementCopyName)
                {
                    var mirror = GetComponent<MirrorMov>();
                    mirror.target = child;
                    mirror.enabled = true;
                    break;
                }
                else
                    lookForTarget(child);
            }
        }


        lookForTarget(TargetParent);

    }

    void HideUntilNext(Vector3 goal, int nextStart)
    {
        //return;

        //if (index == danceParts.importantParts.Count)
        //    return;


        float t = nextStart * beat.tempo + beatsToTarget * tempo - (beat.AudioSourceTime()); //(beatCount * tempo + (beat.source.time - beatCount * tempo));
        //print("HI " + t + " " + part.startBeat + " " + beatsToTarget + " " + danceParts.tempoMultiply + " " + tempo + " " + beat.source.time);
        goalIndicator.transform.DOKill();
        goalIndicator.transform.DOMove(goal, t);
        goalIndicator.HideAndShow(t);
    }

    void addIndicatorPoint()
    {
        var M1 = parent.worldToLocalMatrix;
        var M2 = target.localToWorldMatrix;
        indicatorPos.Enqueue(M2.MultiplyPoint3x4(M1.MultiplyPoint3x4(transform.position)));
    }
    IEnumerator MoveIndicator()
    {
        yield return new WaitForSeconds(beat.tempo * beat.beatsToTarget);
        while (true)
        {
            goalIndicator.transform.position = indicatorPos.Dequeue();
            yield return null;
        }
    }

    public void MakeString(float startTime, float endTime, float nextStartTime, string style, bool oneBeat)
    {
        print(endTime + " , " + tempo);
        int endBeat = oneBeat ? beatCount + 1 : (beatCount + 1) + (int)((endTime - startTime) / tempo); //(int)((endTime - beat.AudioOffset()) / tempo);
        StartCoroutine(StringMaker(beatCount+1, endBeat, (int)((nextStartTime-beat.AudioOffset()) / tempo)+1, style));
    }

    int stringsBeingDrawn;
    IEnumerator StringMaker(int startBeat, int endBeat, int nextStartTime, string effectName)
    {
        print("lifetime: " +  startBeat + " end " + endBeat + " , " + beat.AudioOffset());
        while (beatCount < startBeat)
            yield return null;


        //goalIndicator.transform.DOKill();
        stringsBeingDrawn++;
        var startPoint = Instantiate(stringStartPoint,parent);
        if (!beat.showLine)
            startPoint.transform.localScale = Vector3.zero;
        LineRenderer line_;// line, line_;
        //line = Instantiate(targetLine, parent).GetComponent<LineRenderer>();
        if (beat.showLine)
            line_ = Instantiate(targetLine, parent).GetComponent<LineRenderer>();
        else
            line_ = null;
        //line.material = goalIndicator.normal;

        var effect = Instantiate(beat.styleLib.styles[effectName], goalIndicator.transform).GetComponent<stringVariables>();

        bool neverDestroyed = true;
        bool first = true;

        int destroyed = 0;
        int latestBeat = 0;


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
            if (lifeTimes.Count == 0)
                return;
            int maxL = Mathf.Min(endPoints.Count, Mathf.Min(previewNumber, (int) (Mathf.Max(0, lifeTimes[0] - ((beatsToTarget-previewLenght) * tempo)) / resolution)));
            //line.positionCount = maxL;
            //line.SetPositions(endPoints.GetRange(0, maxL).ToArray());
            if (beat.showLine)
            {
                line_.positionCount = danceString.Count;
                line_.SetPositions(danceString.ToArray());
            }

            if (maxL > 0 && !neverDestroyed)
            {
                Vector3 p = neverDestroyed ? danceString[0] : goalIndicator.transform.position;
                Vector3 viewPoint = Camera.main.WorldToViewportPoint(p);

                if(beat.drawRays)
                {
                    
                        beat.rays.DrawRays(p, activeColor);

                    
                }
                
            }
        }

        void UpdateStartPoint()
        {
            if (!beat.showLine)
                return;
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

                        HideUntilNext(endPoints[0], startBeat); /////HOX
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



        while (Mathf.Min(1, (lifeTimes[lifeTimes.Count - 1] / resolution) / beatsToTarget / stringResolution) != 1)
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
                    goalIndicator.colliderEnable(true);

                    Vector3 goal;
                    if (lifeTimes.Count > i + 1)
                    {
                        lerpProcent = Mathf.Min(1, 1 - ((resolution * beatsToTarget * stringResolution - lifeTimes[i + 1]) / resolution));// Mathf.Min(1, (lifeTimes[i + 1] / resolution) / beatsToTarget / stringResolution);
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
                    bool pointRemoved = false;
                    while (i > -1)
                    {
                        neverDestroyed = false;
                        lifeTimes.RemoveAt(0);
                        startPoints.RemoveAt(0);
                        endPoints.RemoveAt(0);
                        danceString.RemoveAt(0);
                        destroyed++;
                        pointRemoved = true;
                        i--;
                    }

                    if (goalIndicator.active)
                    {
                        if (goalIndicator.neverTouched)
                        {
                            effect.StartEvent(goalIndicator);
                            latestBeat = doupleBeatCount+1;
                        }
                        else if(doupleBeatCount > latestBeat)
                        {

                            effect.DuringEvent();
                            latestBeat = doupleBeatCount;
                        }

                        if (!effect.activated)
                        {
                            effect.activated = true;
                        }
                    }
                    //else if (effect.activated)
                    //{
                    //    effect.activated = false;
                    //}

                    break;


                }
                //Draw.ingame.Line(previous, danceString[i]);
                //Draw.ingame.WireSphere(endPoints[i], 0.01f / i, Color.black);
                //if ((i+destroyed) % stringResolution == 0)
                //    Draw.ingame.WireSphere(danceString[i], 0.02f,Color.gray);
                previous = danceString[i];

            }

            for (int i = 0; i < endPoints.Count; i++)
            {
                //Draw.ingame.WireSphere(endPoints[i], 0.001f, Color.black);
            }

            //lineRenderer.

            

            UpdateStringVisuals();
            UpdateStartPoint();

        }

        if (goalIndicator.active)
        {
            if(startBeat != endBeat)
                effect.EndEvent(goalIndicator);
        }
        goalIndicator.colliderEnable(false);


        //if (points.Count > 0)
        //    points.Dequeue();
        //goalIndicator.visuals.transform.DOScale(0, 0.1f);
        //Destroy(line.gameObject);
        if(line_ != null)
            Destroy(line_.gameObject);
        Destroy(startPoint);
        Destroy(effect.gameObject);
        stringsBeingDrawn--; ;
        if (stringsBeingDrawn > 0)
            HideUntilNext(points.Dequeue(), nextStartTime); //HOX
        else
            goalIndicator.visuals.transform.DOScale(0f, 0.1f);

        goalIndicator.neverTouched = true;

    }


    private void OnDestroy()
    {
        Destroy(goalIndicator.arrow.gameObject);
        Destroy(goalIndicator.gameObject);
       
    }
}
