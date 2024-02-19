using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EliCDavis.RecordAndPlay.Record;
using EliCDavis.RecordAndPlay.Playback;
using EliCDavis.RecordAndPlay;
using System.IO;
using UnityEngine.Android;
using System;
using UnityEngine.Events;

public class PlaybackManager : MonoBehaviour, IActorBuilder, IPlaybackCustomEventHandler
{
    public GameObject[] trackedObjects;
    public GameObject recordingVisualsCanvas;
    [Space]
    public bool DISABLE_MOTION_TRACKING;
    public OvrAvatar avatar;
    public OVRCameraRig cameraRig;
    [Space]
    [SerializeField]
    Recording recording;
    private Recorder recorder;
    private PlaybackBehavior playback;
    string path;
    int recordingNum = 0;

    InputManager inputManager;

    void Awake()
    {
#if UNITY_EDITOR
        if (DISABLE_MOTION_TRACKING)
        {
            avatar.enabled = false;
            cameraRig.enabled = false;
        }
#endif
    }
    // Start is called before the first frame update
    void Start()
    {
        inputManager = transform.GetComponent<InputManager>();
        recordingVisualsCanvas.SetActive(false);


        recorder = ScriptableObject.CreateInstance<Recorder>();
        for (int i = 0; i < trackedObjects.Length; i++)
            SubjectBehavior.Build(trackedObjects[i], recorder, trackedObjects[i].transform.name);


#if PLATFORM_ANDROID

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        path = Application.persistentDataPath;
#endif


#if UNITY_EDITOR
        if (recording != null)
            playback = PlaybackBehavior.Build( recording,this,this,false);
        path = Application.dataPath;
#endif


    }

    public RecordingState RecorderState()
    {
        return recorder.CurrentState();
    }

    public void CaptureEvent(string eventName, string eventData =  "")
    {
        recorder.CaptureCustomEvent(eventName,eventData);
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
            Debug.Log("컴컴");
           
            if(state == RecordingState.Stopped)
            {
                recordingVisualsCanvas.SetActive(true);

                Debug.Log("컴컴 Start");
                recorder.Start();
                if (playback != null)
                {
                    playback.Stop();
                    Destroy(playback.gameObject);
                }
            }
            else
            {
                recordingVisualsCanvas.SetActive(false);

                Debug.Log("컴컴 stop");
                playback = PlaybackBehavior.Build(recorder.Finish(), this, this, true);

                string date = "/" + System.DateTime.Now.ToString("MM_dd_HH_mm");
                if (!System.IO.Directory.Exists(path + date))
                    System.IO.Directory.CreateDirectory(path + date);
                SaveRecording(path + date);

                
            }
          
        }
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

    void SaveRecording(string path_)
    {
        //SaveToAssets(playback.GetRecording(), "Demo");
        using (FileStream fs = File.Create(string.Format("{0}/InputData_" + recordingNum +".rap", path_)))
        {
            recordingNum++;
            var rec = playback.GetRecording();
            rec.RecordingName = "InputData";
            EliCDavis.RecordAndPlay.IO.Packager.Package(fs, rec);
        }
    }

    public Actor Build(int subjectId, string subjectName, Dictionary<string, string> metadata)
    {
        for(int i = 0; i < trackedObjects.Length; i++)
            if(trackedObjects[i].transform.name == subjectName)
                return new Actor(trackedObjects[i], this);

        return new Actor(GameObject.CreatePrimitive(PrimitiveType.Sphere), this);
    }

    public void OnCustomEvent(SubjectRecording subject, CustomEventCapture customEvent)
    {
        var input_index = inputManager.inputsAndEvents.FindIndex(d => d.buttonName == customEvent.Name);
        if(input_index != -1)
        {
            var input = inputManager.inputsAndEvents[input_index];
            if(customEvent.Contents == "press")
            {
                input.press_events.Invoke();
                input.isPressed = true;
            }
            else
            {
                input.release_events.Invoke();
                input.isPressed = false;
            }
        }
    }
}
