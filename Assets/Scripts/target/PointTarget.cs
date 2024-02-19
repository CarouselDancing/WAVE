using DG.Tweening;
using EliCDavis.RecordAndPlay.Record;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTarget : MonoBehaviour
{
    public Material normal;
    public Material activated;
    public GameObject visuals;
    public string targetName;
    bool canBeActivated;
    public bool active;
    public bool neverTouched;
    public Transform arrow;
    public Vector3 arrowScale;
    public MovementRecorder recorder;
    MeshRenderer mr;
    // Start is called before the first frame update
    void Awake()
    {
        recorder = GameObject.FindObjectOfType<MovementRecorder>();
        canBeActivated = true;
        mr = visuals.GetComponent<MeshRenderer>();
        mr.material = normal;
        visuals.transform.DOScale(0.5f, 0.2f);
        active = false;

    }

    private void Start()
    {
        recorder.AddObject(gameObject, targetName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("ENTERED ZONE");
        if(targetName == other.name && canBeActivated)
        {
            print("IS THE THING");
            mr.material = activated;
            visuals.transform.DOScale(1f, 0.2f);
            arrow.DOScale(0, 0.3f);
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetName == other.name && canBeActivated)
        {
            mr.material = normal;
            visuals.transform.DOScale(0.5f, 0.2f);
            active = false;
            arrow.DOScale(arrowScale, 0.3f);

        }
    }

    public bool isHit()
    {
        return active;
    }

    public void HideAndShow(float time)
    {
        var seq = DOTween.Sequence();
        
        
        colliderEnable(false);
        arrow.DOScale(arrowScale, 0.3f);
        mr.material = normal;
        active = false;
        seq.Append(visuals.transform.DOScale(0, 0.1f));
        seq.Append(visuals.transform.DOScale(1, Mathf.Max(0, time - 0.1f)));
        seq.OnComplete(() => colliderEnable(true));

    }

    public void colliderEnable(bool enable)
    {
        if(canBeActivated != enable)
            recorder.CaptureEvent(targetName + "_target", enable.ToString());
        canBeActivated = enable;
        transform.GetComponent<Collider>().enabled = enable;
    }
}
