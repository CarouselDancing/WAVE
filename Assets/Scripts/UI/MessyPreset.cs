using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessyPreset : MonoBehaviour
{
    public string formationName;
    public Vector3 teacherPos = new Vector3(0,3,12);
    public BeatPlayer bp;
    public Vector3[] points;
    public Transform[] bezPoints;
    public ToggleButton button;

    public Bezier[] bezs;
    public bool dancerSameTime;

    public int[] numerOfDancers;
    public SetValue[] valueSetters;

    public int[] speeds;

    public int speed = 8;
    public void OnEnable()
    {


        for (int i = 0; i < points.Length; i++)
        {
            bezPoints[i].position = points[i];
        }

        bezPoints[0].parent.GetComponent<Bezier>().speed = speeds[0];
        bezPoints[2].parent.GetComponent<Bezier>().speed = speeds[1];
        bezPoints[5].parent.GetComponent<Bezier>().speed = speeds[2];


        button.Toggle();

       


        foreach(var b in bezs)
        {
            b.addPointToEnd = dancerSameTime;
        }


        for (int i = 0; i < valueSetters.Length; i++)
        {
            var setter = valueSetters[i];
            setter.value = numerOfDancers[i];
            setter.bez.UpdatePoint(numerOfDancers[i]);
        }

        bp.beatsToTarget = speed;

        if (bp.danceQ != null)
        {
            bp.danceQ.UpdateAmount(0);
            bp.danceQ.UpdateAmount(1);
            bp.danceQ.UpdateAmount(2);
            bp.danceQ.UpdateAnimations();
        }

        
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
