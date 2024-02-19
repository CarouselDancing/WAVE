using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
public class Bezier : MonoBehaviour
{
    public Transform[] b_points;
    public Vector3[] points;
    public int pointAmount;
    public bool addPointToEnd;
    public BeatPlayer bp;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int Amount()
    {
        return addPointToEnd && pointAmount > 0 ? pointAmount + 1 : pointAmount;
    }
    // Update is called once per frame
    void Update()
    {
        updatePoints();
    }

    public void updatePoints()
    {
        int amount = pointAmount;

        points = addPointToEnd && pointAmount > 0 ? new Vector3[amount + 1] : new Vector3[amount];

        int amount_ = 100;
        float step = 1 / (1 + amount_ * 1.0f);

        var points_ = new Vector3[amount_];

        float sum = 0;
        for(int i = 1; i <= amount_; i++)
        {
            float t = step * i;

            Vector3 v;

            v = Mathf.Pow(1 - t, 3) * b_points[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * b_points[1].position +
                3 * (1-t)* Mathf.Pow(t, 2) * b_points[1].position +
                Mathf.Pow(t, 3) * b_points[2].position;

            points_[i-1] = new Vector3(v.x, 0, v.z);
            if (i == 1)
                sum += Vector3.Distance(new Vector3(b_points[0].position.x, 0, b_points[0].position.z), points_[i - 1]);
            else
                sum += Vector3.Distance(points_[i - 2], points_[i - 1]);
        }

        float f = addPointToEnd && pointAmount > 0 ? points.Length : points.Length + 1;

        step = sum / f;

        for(int i = 0; i < points.Length;i++)
        {
            float sum_ = 0;
            int p0 = 0;
            for(int p1 = 1; p1 < amount_; p0 = p1++)
            {
                sum_ += Vector3.Distance(points_[p0], points_[p1]);
                if(sum_ > step * (i + 1))
                {
                    points[i] = points_[p1];
                    break;
                }
            }
        }

        if (addPointToEnd && pointAmount > 0)
            points[points.Length - 1] = new Vector3(b_points[2].position.x, 0, b_points[2].position.z);

        foreach (var vec in points)
        {
           
            Draw.ingame.WireSphere(vec, 0.05f,Color.yellow);
        }
    }

    public void UpdatePoint( int newValue)
    {
        pointAmount = newValue;
        updatePoints();
        if (bp.danceQ != null)
        {
            bp.danceQ.UpdateAmount(transform.GetChild(0).GetComponent<MoveObject>().index);
            bp.danceQ.UpdateAnimations();
        }
    }
}
