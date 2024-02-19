using EliCDavis.RecordAndPlay.Record;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    
    public List<InputEvents> inputsAndEvents;
    PlaybackManager playbackM;
    [Serializable]
    public struct InputEvents
    {
        public string buttonName;
        public OVRInput.RawButton inputID;
        [Space]
        public UnityEvent press_events;
        public UnityEvent release_events;
        [HideInInspector]
        public bool isPressed;

    }
    // Start is called before the first frame update
    void Start()
    {
        playbackM = GetComponent<PlaybackManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
        for (int i = 0; i < inputsAndEvents.Count; i++)
        {
            var input = inputsAndEvents[i];
            if (OVRInput.GetDown(input.inputID))
            {
                
                //RECORD BUTTON INPUTS
                if (playbackM.RecorderState() != RecordingState.Stopped)
                {
                    playbackM.CaptureEvent(input.buttonName,"press");
                }

                //invoke input's events
                input.press_events.Invoke();

                input.isPressed = true;
            }
            if (OVRInput.GetUp(input.inputID))
            {

                //RECORD BUTTON INPUTS
                if (playbackM.RecorderState() != RecordingState.Stopped)
                {
                    playbackM.CaptureEvent(input.buttonName, "release");
                }

                //invoke input's events
                input.release_events.Invoke();

                input.isPressed = false;
            }
        }
    }

}
