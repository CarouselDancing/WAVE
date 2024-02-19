using Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRaysInCircle : MonoBehaviour
{
    public Transform towards;
    public Transform towards2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
      // DrawRays(towards2.position, Color.red);
    }

    public void DrawRays(Vector3 pos, Color color_)
    {
        using (Draw.ingame.WithLineWidth(1f))
        {
            //Draw.ingame.Line(pos, towards.position, Color.red);
            //Draw.ingame.Line(pos, pos + Vector3.up, Color.red);

            Vector3 cross = Vector3.Cross(pos - towards.position, Vector3.up);
            var angle = Vector3.Angle(pos - transform.parent.position, towards.position - transform.parent.position);
            for (int i = 0; i < 36; i++)
            {
                Vector3 vector = Quaternion.AngleAxis(-10 * i, cross) * (pos - towards.position);
                float a =Mathf.Max(0, 0.70f - Mathf.Clamp01(  (90 - angle) / 90)); //0.2f;
                var c = new Color(color_.r, color_.g, color_.b, Mathf.Min(0.5f, a));
                Draw.ingame.Line(pos, pos + vector,c);
            }


        }
    }
}
