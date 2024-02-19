using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EliCDavis.RecordAndPlay.Record;
using EliCDavis.RecordAndPlay.Playback;
using EliCDavis.RecordAndPlay;
using System.IO;
using UnityEngine.Android;
using System;

public class MovementRecorder : MonoBehaviour, IActorBuilder, IPlaybackCustomEventHandler
{
    public GameObject[] trackedObjects;
    [Space]
    [SerializeField]
    Recording recording;
    private Recorder recorder;
    private PlaybackBehavior playback;
    string path;

    public GameObject targetIndicator;

    // Start is called before the first frame update
    void Start()
    {
        recorder = ScriptableObject.CreateInstance<Recorder>();
        AddObject(trackedObjects[0], "head");
        AddObject(trackedObjects[1], "R_hand");
        AddObject(trackedObjects[2], "L_hand");

        AddObject(trackedObjects[3], "t_head");
        AddObject(trackedObjects[4], "t_R_hand");
        AddObject(trackedObjects[5], "t_L_hand");

#if PLATFORM_ANDROID

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        path = Application.persistentDataPath;
#endif


#if UNITY_EDITOR
        if (recording != null)
            playback = PlaybackBehavior.Build(recording, this, this, false);
        path = Application.dataPath;
#endif


    }

    public void startRecorder()
    {
        recorder = ScriptableObject.CreateInstance<Recorder>();
        AddObject(trackedObjects[0], "head");
        AddObject(trackedObjects[1], "R_hand");
        AddObject(trackedObjects[2], "L_hand");

        AddObject(trackedObjects[3], "t_head");
        AddObject(trackedObjects[4], "t_R_hand");
        AddObject(trackedObjects[5], "t_L_hand");

        recorder.Start();
    }

    public void AddObject(GameObject g, string name)
    {
        SubjectBehavior.Build(g, recorder, name);
    }

    public RecordingState RecorderState()
    {
        return recorder.CurrentState();
    }

    public void CaptureEvent(string eventName, string eventData = "")
    {
        recorder.CaptureCustomEvent(eventName, eventData);
    }
    // Update is called once per frame
    void Update()
    {
        var state = recorder.CurrentState();


        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            print("moi");
        }
        //SAVE RECORDINGS
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("ÄÄÄÄ");
              AddObject(trackedObjects[1], "TEST2");
            //if (state == RecordingState.Stopped)
            //{

            //    Debug.Log("ÄÄÄÄ Start");
            //    recorder.Start();
            //    if (playback != null)
            //    {
            //        playback.Stop();
            //        Destroy(playback.gameObject);
            //    }
            //}
            //else
            //{

            //    Debug.Log("ÄÄÄÄ stop");
            //    playback = PlaybackBehavior.Build(recorder.Finish(), this, this, true);

            //    string date = "/" + System.DateTime.Now.ToString("MM_dd_HH_mm");
            //    if (!System.IO.Directory.Exists(path + date))
            //        System.IO.Directory.CreateDirectory(path + date);
            //    SaveRecording(path + date);


            //}

        }
    }

    public Recorder GetRecorder()
    {
        return recorder;
    }
    void OnGUI()
    {
        switch (recorder.CurrentState())
        {
            case RecordingState.Recording:
                if (recorder.CurrentlyRecording() && GUI.Button(new Rect(10, 10, 120, 25), "Pause"))
                {
                    recorder.Pause();
                }
                if (GUI.Button(new Rect(10, 40, 120, 25), "Finish"))
                {
                    playback = PlaybackBehavior.Build(recorder.Finish(), this, this, true);
                }
                break;

            case RecordingState.Paused:
                if (recorder.CurrentlyPaused() && GUI.Button(new Rect(10, 10, 120, 25), "Resume"))
                {
                    recorder.Resume();
                }

                if (GUI.Button(new Rect(10, 40, 120, 25), "Finish"))
                {
                    playback = PlaybackBehavior.Build(recorder.Finish(), this, this, true);
                }
                break;

            case RecordingState.Stopped:
                if (GUI.Button(new Rect(10, 10, 120, 25), "Start Recording"))
                {
                    //recorder.ClearSubjects();
                    //SetUpObjectsToTrack();
                    recorder.Start();
                    if (playback != null)
                    {
                        playback.Stop();
                        Destroy(playback.gameObject);
                    }
                }
                if (playback != null)
                {
                    GUI.Box(new Rect(10, 50, 120, 250), "Playback");
                    if (playback.CurrentlyPlaying() == false && GUI.Button(new Rect(15, 75, 110, 25), "Start"))
                    {
                        playback.Play();
                    }

                    if (playback.CurrentlyPlaying() && GUI.Button(new Rect(15, 75, 110, 25), "Pause"))
                    {
                        playback.Pause();
                    }

                    GUI.Label(new Rect(55, 105, 100, 30), "Time");
                    GUI.Label(new Rect(55, 125, 100, 30), playback.GetTimeThroughPlayback().ToString("0.00"));
                    playback.SetTimeThroughPlayback(GUI.HorizontalSlider(new Rect(15, 150, 100, 30), playback.GetTimeThroughPlayback(), 0.0F, playback.RecordingDuration()));

                    GUI.Label(new Rect(20, 170, 100, 30), "Playback Speed");
                    GUI.Label(new Rect(55, 190, 100, 30), playback.GetPlaybackSpeed().ToString("0.00"));
                    playback.SetPlaybackSpeed(GUI.HorizontalSlider(new Rect(15, 215, 100, 30), playback.GetPlaybackSpeed(), -8, 8));

                    if (GUI.Button(new Rect(15, 250, 110, 25), "Save"))
                    {

                        SaveRecording(path);

                    }
                }
                break;
        }
    }

    public void SaveMovements()
    {
        recorder.Pause();
        playback = PlaybackBehavior.Build(recorder.Finish(), this, this, true);
        SaveRecording(GameObject.FindObjectOfType<PlayerInfo>().GetDirectory());
    }

    void SaveRecording(string path_)
    {
        print(path_);
        //SaveToAssets(playback.GetRecording(), "Demo");
        using (FileStream fs = File.Create(string.Format("{0}/movementData" + ".rap", path_)))
        {
            //recordingNum++;
            var rec = playback.GetRecording();
            rec.RecordingName = "MovementData";
            EliCDavis.RecordAndPlay.IO.Packager.Package(fs, rec);
        }
    }

    public Actor Build(int subjectId, string subjectName, Dictionary<string, string> metadata)
    {
        //for (int i = 0; i < trackedObjects.Length; i++)
        //    if (trackedObjects[i].transform.name == subjectName)
        //        return new Actor(trackedObjects[i], this);

        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        switch (subjectName)
        {
            case "head":
                g.transform.localScale = Vector3.one * 0.2f;
                g.name = "head";
                break;
            case "R_hand":
                g.transform.localScale = Vector3.one * 0.1f;
                g.name = "R_hand";
                break;
            case "L_hand":
                g.transform.localScale = Vector3.one * 0.1f;
                g.name = "L_hand";
                break;
            case "t_head":
                g.transform.localScale = Vector3.one * 0.06f;
                g.name = "t_head";
                break;
            case "t_R_hand":
                g.transform.localScale = Vector3.one * 0.03f;
                g.name = "t_R_hand";
                break;
            case "t_L_hand":
                g.transform.localScale = Vector3.one * 0.03f;
                g.name = "t_L_hand";
                break;
            default:
                g = Instantiate(targetIndicator, transform);
                g.name = subjectName;
                break;

        }
        return new Actor(g, this);
    }

    public void OnCustomEvent(SubjectRecording subject, CustomEventCapture customEvent)
    {
        Transform p = GameObject.Find("PLAYBACK OBJECT").transform;
        var name = customEvent.Name[0] + "";

        for(int i = 0; i < p.childCount; i++)
        {
            if(p.GetChild(i).name == name)
            {
                p = p.GetChild(i);
            }

        }
        print(customEvent.Name);
        print(customEvent.Contents);


        if (bool.Parse(customEvent.Contents))
            p.localScale = Vector3.one * 0.3f;
        else
            p.localScale = Vector3.one * 0.1f;




    }
}