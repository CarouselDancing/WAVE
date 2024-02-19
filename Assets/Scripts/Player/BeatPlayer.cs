using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class BeatPlayer : MonoBehaviour
{

    public Transform videoCharacter;
    public AudioSource ambient;

    [Header("from level")]
    public LevelInfo levelInfo;
    public PlayableDirector director;
    public int beatsToTarget;
    public Transform CURVES;

    [SerializeField] Material floorMaterial;
    public Material invisibleMaterial;
    TimelineAsset timeline;
    TrackAsset audioTrack;
    TimelineClip audioClip;
    TimelineClip animClip;
    [Space]
    public DrawRaysInCircle rays;
    public MovementRecorder recorder;
    public Transform Player;
    public GameObject puppetPrefab;
    public Animator playerShadow;
    public XRController controller;
    public MoveStyleLibrary styleLib;
    public bool useTimeline;
    public UseQuestionaire questionaireUI;
    //public Animator animator;
    [HideInInspector] public Track track;
   
    //public GameObject beatIndicator;
    
    [HideInInspector]
    public float tempo;
    [HideInInspector]
    public int beatCount;
    [HideInInspector]
    public PlayerInfo.GameMode mode;
    AudioSource source;
   
    [HideInInspector]
    public DanceQueue danceQ;
    [HideInInspector]
    public bool playing;
    float offset;

    bool play;
    [HideInInspector]
    public bool canPause;
    bool pressedOnce;
    int pressedPlay;
    Coroutine coroutine, coroutine2;
    bool danceQHidden;
    bool musicCanPlay;
    bool started;
    public bool hideDanceQ;

    public ChangeFormation formation;

    //game mode settings
    public bool showTeacher, showPartners, realTime, showQ,showLine;
    public float teacherScale = 1;

    public bool drawRays;
    private void OnEnable()
    {
        recorder.startRecorder();

        exitCounter = 0;

        playerShadow.transform.position = Vector3.zero;
        playerShadow.gameObject.SetActive(true);

        if(!showTeacher)
            levelInfo.teacherModel.material = invisibleMaterial;

        Player.localScale = Vector3.one * (GameObject.FindObjectOfType<PlayerInfo>().height / levelInfo.eyeLevel) / teacherScale;
        playerShadow.transform.localScale = Vector3.one * (GameObject.FindObjectOfType<PlayerInfo>().height / levelInfo.eyeLevel);
        playerShadow.transform.position = Vector3.zero;
        playerShadow.transform.rotation = Quaternion.Euler(0, 0, 0);


        danceQ = levelInfo.transform.GetChild(levelInfo.transform.childCount - 1).GetComponent<DanceQueue>();
        musicCanPlay = false;
        canPause = false;
        pressedOnce = false;
        started = false;
        pressedPlay = 0;
        play = false;

        tempo = 60f / levelInfo.BPM * levelInfo.beatDivide;

        timeline = (TimelineAsset)director.playableAsset;
        audioTrack = timeline.GetOutputTracks().FirstOrDefault(t => t is AudioTrack);
        audioClip = audioTrack.GetClips().FirstOrDefault();

        var animTrack = timeline.GetOutputTracks().FirstOrDefault(t => t is AnimationTrack);

        animClip = animTrack.GetClips().FirstOrDefault();
        source = GetComponent<AudioSource>();

        if (realTime)
        {
            print("REAL TIME");
            var directorSource = director.transform.GetComponent<AudioSource>();
            directorSource.volume = 0;
            source.clip = directorSource.clip;
            print("WAIT FOR " + levelInfo.beatsToTarget * tempo + (float)audioClip.start);
            StartCoroutine( WaitAndPlay(levelInfo.beatsToTarget * tempo + (float)audioClip.start));
        }
       
        //source.playOnAwake = false;

        //if(!useTimeline)
        //    source.clip = track.music;

        if (useTimeline)
            offset = (float)audioClip.start;
        //else
        //    offset = track.offset;
        playing = true;

      
    }

    public float AnimTime()
    {
        return (float) (director.time - animClip.start);
    }

    //public float AnimLenght()
    //{
    //    return (float)( animClip.end - animClip.start);
    //}
  
    public float AudioOffset()
    {
        return offset;
    }
    public float AudioSourceTime()
    {
        if (!useTimeline)
            return source.time;
        else if (levelInfo.useOffset)
            return (float)(director.time - audioClip.start) - offset;
        else
            return (float)(director.time - audioClip.start);
    }

    float exitCounter = 0;
    void Update()
    {
        if (!playing)
            return;
        if(Mathf.Abs((float)(director.time - audioClip.end)) < 0.5f || Input.GetKeyDown(KeyCode.X) || exitCounter > 3f)
        {

            GameObject.FindObjectOfType<PlayerInfo>().CreateLevelDirectory();

            ambient.DOKill();
            ambient.DOFade(1, 4);

            floorMaterial.DOKill();
            floorMaterial.DOFloat(3, "_Zvalue", 15);
            playing = false;
            //print(mode.questions.ID);
            questionaireUI.questionaire = mode.questions;
            questionaireUI.gameObject.SetActive(true);
            Destroy(levelInfo.gameObject, 1f);
            playerShadow.gameObject.SetActive(false);
            source.Stop();

            recorder.SaveMovements();

            gameObject.SetActive(false);
        }
        bool pressed;

        //if (danceQHidden != showQ) 
        //     danceQHidden = danceQ.HideDancers(showQ);

        if ((controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed) && pressed))
        {
            exitCounter += Time.deltaTime;
        }
        
        

        if (!started)//|| (controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed) && pressed) || Input.GetKeyDown(KeyCode.Space)
        {

            if (!started)// || !pressedOnce)
            {
                started = true;

                play = !play;
                pressedPlay++;
                pressedOnce = true;
                if (play)
                {
                   

                    if (useTimeline)
                    {
                        director.Play();
                    }

                    if (showPartners)
                        levelInfo.realTimePartners.SetActive(true);

                    playerShadow.speed = 1;

                   

                    if (showQ)
                    {
                        if (danceQ.transform.childCount == 0)
                            danceQ.StartDancers(levelInfo.animationName,this);
                        else
                            danceQ.SetDancersSpeed(1);
                    }
                    else
                    {
                        if (!playerShadow.GetCurrentAnimatorStateInfo(0).IsName(levelInfo.animationName))
                            coroutine2 = StartCoroutine(WaitAndPlayAnimation(tempo * beatsToTarget, pressedPlay));
                    }
                    
                }
                //else
                //{
                //    if (canPause)
                //    {
                //        if (useTimeline)
                //        {
                //            director.Pause();
                //        }
                    
                //        playerShadow.speed = 0;

                //        if (coroutine2 != null)
                //            StopCoroutine(coroutine2);

                //        if(showQ)
                //            danceQ.SetDancersSpeed(0);
                //    }
                   
                //}
            }
           
        }
        else //if (controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed))
        {
            pressedOnce = false;
        }

        if(tempo <= (AudioSourceTime()) - beatCount * tempo)
        {

            beatCount++;
            //var g = Instantiate(beatIndicator);
            //Destroy(g, 1);
        }
    }

    void spawnPartner(Vector3 pos)
    {

    }

    public void setCanPause(bool value)
    {
        canPause = value;
    }
    IEnumerator WaitAndPlay(float time)
    {
        yield return new WaitForSeconds(time);
        //if(pressedPlay == play)
        //{
        //    musicCanPlay = true;
            source.Play();
        //}
       
    }

    IEnumerator WaitAndPlayAnimation(float time, int play)
    {
        yield return new WaitForSeconds(time);
        //if (pressedPlay == play)
        {
            playerShadow.Play(levelInfo.animationName);
            if (showPartners)
            {
                foreach (Transform child in levelInfo.realTimePartners.transform)
                {
                    child.GetComponent<Animator>().Play(levelInfo.animationName);
                }
            }
         
        }

    }

}
