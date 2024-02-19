using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
using DG.Tweening;
public class RayCircle : MonoBehaviour
{
    public float R;
    public Color color;
    public Transform target;
    public Transform trigger;
    public float y;
    public int numberOfRays;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool outside;
    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(new Vector2(target.position.x, target.position.z), new Vector2(trigger.position.x, trigger.position.z));
        
        if (dist < R)
        {
            if(!outside)
                
            outside = true;
            return;

        }

        color.a = (dist - R) * 10 ;

        Vector3 center = new Vector3(target.position.x, y, target.position.z);
        Draw.ingame.Circle(center , Vector3.up, R, color);
        float angle = 360 * 1.0f / numberOfRays;

        for(int i = 0; i < numberOfRays; i++)
        {
            Vector3 vector = Vector3.forward;
            vector = Quaternion.AngleAxis(angle * i, Vector3.up) * vector;

            Vector3 start = center + (vector * R) + (vector * 0.1f);
            Vector3 end = center + (vector * 25);

            Draw.ingame.Line(start, end, color);
        }
    }
}
