using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
using DG.Tweening;

public class ToggleButton : MonoBehaviour
{
    public  BeatPlayer bp;
    public EffectSettings es;
    public List<GameObject> objs;

    Transform eyes;
    public float width1 = 5;
    public float width2= 1;
    public Color c0 = Color.white;

    public Color c;
    bool on;
    public  Transform p0;
    public Transform p1;

    public bool SetRays = false;
    public bool isMover = false;
    // Start is called before the first frame update
    void Start()
    {
        es = GameObject.FindObjectOfType<EffectSettings>();

        eyes = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(eyes);

        using (Draw.ingame.WithLineWidth(width1))
        {
            Draw.ingame.Circle(transform.position, transform.forward, transform.localScale.x *  0.1f,c0);
           
        }
        Draw.ingame.Arrow(p1.position, p0.position, transform.forward, 0.04f);
        if (on)
        {
            using (Draw.ingame.WithLineWidth(width2))
            {
                Draw.ingame.Circle(transform.position, transform.forward, transform.localScale.x * 0.1f * 0.7f, c);
            }
        }
        

    }

    public void Toggle2()
    {
        on = !on;
        if (on)
            es.toggle = this;
        else
            es.toggle = null;

        foreach (var o in objs)
        {
            o.SetActive(!o.activeSelf);
        }

        if (SetRays)
        {
            bp.drawRays = !bp.drawRays;
        }
    }
    public void Toggle()
    {
        transform.localScale = Vector3.one;
        transform.DOShakeScale(0.2f);

        if(es.toggle != null && es.toggle.gameObject.GetInstanceID() != gameObject.GetInstanceID() && !es.toggle.SetRays && !es.toggle.isMover)
        {
            es.toggle.Toggle2();
        }

        Toggle2();
    }
}
