using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class stringVariables : MonoBehaviour
{
    public Vector3 startPos, currentPos;
    public Transform playerPoint;
    public UnityEvent start;
    public UnityEvent during;
    public UnityEvent end;
    public float timeBetweenEvents;
    public bool activated;
    float timer;
    bool canPlayEvent;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > timeBetweenEvents)
        {
            timer = 0;
            canPlayEvent = true;
        }
        timer += Time.deltaTime;
    }

    public void setVariables(Transform player, Vector3 startPos_, Vector3 currentPos_)
    {
        playerPoint = player;
        startPos = startPos_;
        currentPos = currentPos_;
    }

    public void StartEvent(PointTarget t)
    {
        t.neverTouched = false;
        start.Invoke();
    }
    public void DuringEvent()
    {
        //if (!canPlayEvent)
        //    return;
        canPlayEvent = false;
        during.Invoke();
    }
    public void EndEvent(PointTarget t)
    {
        end.Invoke();
    }
}
