using DG.Tweening;
using Drawing;
using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class PlayPerformance : MonoBehaviour
{

    public Haptics haptics;

    public Color lineColorRr;
    public Color lineColorRl;
    public Color lineColorLr;
    public Color lineColorLl;

    public Material M_Rr;
    public Material M_Rl;
    public Material M_Lr;
    public Material M_Ll;


    public Transform targetHandRr;
    public Transform targetHandRl;
    public Transform targetHandLr;
    public Transform targetHandLl;
    public Transform targetHead;

    //effects
    public GameObject blast;
    public GameObject indicator;

    public GameObject sparkle;
    public MusicPlayer music;
    public bool autoStart;
    [Space]
    public bool useJson;
    public TextAsset performanceJson;
    public PerformanceSobj performanceSobj;
    Performance performance;

    //float tempo;
    //bool onBeat;
    //float beatTimer;
    //float procent;
    int end = 0;
    // Start is called before the first frame update
    void Start()
    {
        print(performanceJson.ToString());
        if (useJson && performanceJson != null)
            performance = JsonUtility.FromJson<Performance>(performanceJson.ToString());
        else
            performance = performanceSobj.Unpack();
        music.MultiplyTempo (1+performance.extraPointsBetweenBeats);


        targetHandRr.DOScale(0, 0);
        targetHandRl.DOScale(0, 0);
        targetHandLr.DOScale(0, 0);
        targetHandLl.DOScale(0, 0);

        if (autoStart)
        {
            StartCoroutine(AutoStart());
        }
    }
    bool playing;
    //bool pause;
    // Update is called once per frame
    void Update()
    {
        //onBeat = beatTimer >= tempo;
        //procent = beatTimer / tempo;
        //if (onBeat)
        //{
        //    beatTimer = 0;
        //}
        //if(!pause)
        //    beatTimer += Time.deltaTime;
        //drawPerformance();

        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick))
        {
            //OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            if (!autoStart &&( !playing || end == 3))
            {
                startPlaying();
            }
            else 
                music.TogglePause();

          
        }
        //if(Input.GetKey(KeyCode.Space))
        //    haptics.Vibrate(VibrationForce.Light, OVRHaptics.RightChannel);
        if (Input.GetKeyDown(KeyCode.Space))
             StartCoroutine( haptics.VibrateTime(VibrationForce.Light,0.1f, OVRHaptics.RightChannel));

    }

    IEnumerator AutoStart()
    {
        while (music.source.time < performance.startTime)
            yield return null;

        startPlaying();
    }
    void startPlaying()
    {
        end = 0;
        playing = true;
        music.ClearBeats();
        PlayParts(targetHandRr, targetHandRl, performance.Rhand, lineColorRr, lineColorRl);
        PlayParts(targetHandLr, targetHandLl, performance.Lhand, lineColorLr, lineColorLl);
        PlayParts(targetHead, targetHead, performance.head, Color.white, Color.white);

        StartCoroutine( updateIndicators(4) );
    }
    void PlayParts(Transform targetR, Transform targetL, List<Performance.Part> parts, Color lineColorR, Color lineColorL)
    {
        StartCoroutine(_PlayParts(targetR, targetL, parts, lineColorR, lineColorL));
    }
    IEnumerator _PlayParts(Transform targetR, Transform targetL, List<Performance.Part> parts, Color lineColorR, Color lineColorL)
    {
        var gL = targetL;
        var gR = targetR;

        int i = 0;
        print(parts.Count);
        while( i < parts.Count)
        {
            yield return StartCoroutine(playMove(targetR, targetL, gR, gL, parts[i], lineColorR, lineColorL));
            i++;
        }

        end++;
    }

    IEnumerator playMove(Transform targetR, Transform targetL, Transform gR, Transform gL, Performance.Part part, Color lineColorR, Color lineColorL)
    {
        int beatCount = 0;
        print("wait : " + part.waitTime);
        var Rvfx = drawSparkle(part.PointsR, lineColorR);
        var Lvfx =  drawSparkle(part.PointsL, lineColorL);

        targetR.DOScale(0, 0.2f);
        targetL.DOScale(0, 0.2f);
        
        targetR.DOMove(part.PointsR[0], music.Tempo() * (part.waitTime));
        targetL.DOMove(part.PointsL[0], music.Tempo() * (part.waitTime));

        while (beatCount <= part.waitTime)
        {
            if (music.OnBeat())
                beatCount++;

            yield return null;

            drawList(part.PointsR, lineColorR,0.5f);
            drawList(part.PointsL, lineColorL, 0.5f);
        }
        int i = 0;
        targetR.DOKill();
        targetL.DOKill();

        targetR.DOScale(1, 0f);
        targetL.DOScale(1, 0f);

        //Rvfx[0].SetBool("stop", true);
        //Lvfx[0].SetBool("stop", true);
        hitPoint(0); 
        while (i < part.PointsL.Count-1)
        {
            drawList(part.PointsR, lineColorR, 0.5f);
            drawList(part.PointsL, lineColorL, 0.5f);

            gR.transform.position = part.PointsR[i] + (part.PointsR[i+1] - part.PointsR[i]) * music.BeatProcent();
            gL.transform.position = part.PointsL[i] + (part.PointsL[i + 1] - part.PointsL[i]) * music.BeatProcent();
            if (music.OnBeat())
            {
                hitPoint(i + 1);
                //Rvfx[i+1].SetBool("stop", true);
                //Lvfx[i+1].SetBool("stop", true);
                i++;
                
            }
            yield return null;
        }

        foreach(var vfx in Rvfx)
        {
            Destroy(vfx.gameObject);
        }
        foreach (var vfx in Lvfx)
        {
            Destroy(vfx.gameObject);
        }
        targetR.DOScale(0, 0.2f);
        targetL.DOScale(0, 0.2f);

        void hitPoint(int i)
        {
            Rvfx[i].SetBool("stop", true);
            Lvfx[i].SetBool("stop", true);
      

            bool insideR = targetR.GetChild(0).GetComponent<MovTarget>().inside;
            bool insideL = targetL.GetChild(0).GetComponent<MovTarget>().inside;
            var channel = OVRHaptics.RightChannel;
            if (targetR.name[0] == 'L')
                channel = OVRHaptics.LeftChannel;
            if(insideR && insideL)
                StartCoroutine(haptics.VibrateTime(VibrationForce.Hard, 0.1f, channel));
            else if (insideL || insideR)
                StartCoroutine(haptics.VibrateTime(VibrationForce.Light, 0.1f, channel));

            if (insideR)
            {
                var g = Instantiate(blast);
                g.transform.position = targetR.position;
            }

            if (insideL)
            {
                var g = Instantiate(blast);
                g.transform.position = targetL.position;
            }

        }
    }

    IEnumerator updateIndicators(int offset)
    {
        int oldBeats = music.BeatCount();
        while (end < 3)
        {
            while (oldBeats == music.BeatCount())
                yield return null;
            findPointThatPlaysAfterNBeats(performance.Rhand, offset, M_Rl, M_Rr);
            findPointThatPlaysAfterNBeats(performance.Lhand, offset, M_Ll, M_Lr);
            findPointThatPlaysAfterNBeats(performance.head, offset, M_Rl, M_Rr);
            yield return null;
            oldBeats = music.BeatCount();
        }
       
    }
    void findPointThatPlaysAfterNBeats(List<Performance.Part> list,int beatOffset, Material L, Material R)
    {
        int beats = music.BeatCount() + beatOffset -1;
        int sum = 0;
        print("B " + beats + " : " + music.BeatCount());
        for(int i = 0; i < list.Count; i++)
        {
            sum += list[i].waitTime;
            for (int p = 0; p < list[i].PointsL.Count; p++)
            {
                sum++;
                if (sum == beats)
                {
                    var gR = Instantiate(indicator);
                    var gL = Instantiate(indicator);

                    gR.GetComponent<MeshRenderer>().material = R;
                    gL.GetComponent<MeshRenderer>().material = L;


                    gR.transform.position = list[i].PointsR[p];
                    gR.transform.DOScale(0, beatOffset * music.Tempo());
                    Destroy(gR, beatOffset * music.Tempo() * 2);

                    gL.transform.position = list[i].PointsL[p];
                    gL.transform.DOScale(0, beatOffset * music.Tempo());
                    Destroy(gL, beatOffset * music.Tempo() * 2);

                }
                if (sum > beats)
                    break;
            }
       
            if (sum > beats)
                break;
        }
    }

    void drawPerformance()
    {
        
        foreach (var l in performance.Rhand)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
        foreach (var l in performance.Lhand)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
        foreach (var l in performance.head)
        {
            drawList(l.PointsL, Color.gray);
            drawList(l.PointsR, Color.white);
        }
    }
    void drawList(List<Vector3> l, Color c, float width = 0.1f)
    {
        if (l == null || l.Count == 0)
            return;

        //Draw.ingame.WireSphere(l[0], 0.02f, c);

        using (Draw.ingame.WithLineWidth(width))
        {
            //Draw.ingame.CatmullRom(l, c);
            for (int i = 1; i < l.Count; i++)
            {
                //Draw.ingame.WireSphere(l[i], 0.01f, c);
                Draw.ingame.Line(l[i], l[i - 1], Color.white);
            }
        }
       
       
    }

    List<VisualEffect> drawSparkle(List<Vector3> l, Color c)
    {
        var gList = new List<VisualEffect>();
        if (l == null || l.Count == 0)
            return gList;

        for (int i = 0; i < l.Count; i++)
        {
             var g = Instantiate(sparkle, l[i], Quaternion.identity) ;
            var effect = g.GetComponent<VisualEffect>();
            effect.SetVector4("color", c);
            gList.Add(effect);
        }

        gList[0].transform.localScale = gList[0].transform.localScale * 2;
        return gList;
    }


}
