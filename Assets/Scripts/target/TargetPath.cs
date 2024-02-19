using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
using OculusSampleFramework;
using System;

public class TargetPath : MonoBehaviour
{
	public bool lockRotation;
	public Transform target;
	public Transform test;
    List<Transform> points;
	List<float> pointsDistance;

	[Range(1,100)] public int reso = 5;
	[Range(-1, 1)] public int direction; 
	List<Vector3> positions;
	List<int> startInds;
	public Color c0;
	public Color c1;

	float distanceSum;
	int index;
	public float complete_time;
	float timer;
	float distance;
    // Start is called before the first frame update
    void Start()
    {

		timer = 0;
	}

    // Update is called once per frame
    void Update()
    {

		timer += (Time.deltaTime *direction);

		UpdateSpline();
		//Draw.ingame.Line(positions[2], ClosestPointOnALine(test.position, points[0].position, points[1].position));



		print(positions.Count + " :: " + " . " );
		distanceSum = 0;

		for (int i = 1; i <positions.Count; i++)
		{
			float d = Vector3.Distance(positions[i - 1], positions[i]);
			distanceSum += d;
		}

		Draw.ingame.WireSphere(positions[0],0.1f,Color.red);
		timer = Mathf.Max(0, timer);
		timer = Mathf.Min(complete_time, timer);

		distance = timer / complete_time;
		if (distance >= 0.99f)
			target.position = points[points.Count - 1].position;

		else if (distance != 0)
		{
			distance = distance * distanceSum;
			float sum = 0;
			for (int i = 1; i < positions.Count; i++)
			{
				sum += Vector3.Distance(positions[i - 1], positions[i]);
				if (sum >= distance)
				{
					int p = 0;
					float d = distance;
					while (d > 0)
					{
						d -= pointsDistance[p];
						p++;
					}
					p--;
					d = Mathf.Abs(d);
					float pro = d / pointsDistance[p];

					var r1 = points[p].rotation.eulerAngles;
					var r2 = points[p+1].rotation.eulerAngles;

					var newRot = new Vector3(r1.x * pro + r2.x * (1 - pro), r1.y * pro + r2.y * (1 - pro), r1.z * pro + r2.z * (1 - pro));

					target.position = positions[i] + (positions[i - 1] - positions[i]).normalized * (sum - distance);
					target.rotation = Quaternion.Euler(newRot);
					break;// 
				}
			}
		}

		using (Draw.ingame.WithLineWidth(1))
		{
			float sum = 0;
			int ind = 0;
			float pro0 = 0;
			Draw.ingame.WireSphere(positions[0], 0.1f);
			for (int i = 0; i < positions.Count - 1; i++)
			{

				if (startInds[i] != ind)
				{
					ind = startInds[i];
					sum = 0;
					pro0 = 0;
				}
				sum += Vector3.Distance(positions[i], positions[i + 1]);

				var t0 = points[startInds[i]];
				var up0 = t0.localToWorldMatrix.MultiplyPoint3x4(Vector3.up) - t0.position;

				var t1 = points[startInds[i] + 1];
				var up1 = t1.localToWorldMatrix.MultiplyPoint3x4(Vector3.up) - t1.position;




				float pro1 = sum / pointsDistance[startInds[i]];


				var rot0 = new Vector3(up0.x * (1 - pro0) + up1.x * pro0,
										up0.y * (1 - pro0) + up1.y * pro0,
										up0.z * (1 - pro0) + up1.z * pro0);

				var rot1 = new Vector3(up0.x * (1 - pro1) + up1.x * pro1,
										up0.y * (1 - pro1) + up1.y * pro1,
										up0.z * (1 - pro1) + up1.z * pro1);



				if (i == 0)
					print(pro0 + " .. " + up0 + " :: " + rot0);

				float dist = 0.2f;
				var p0 = positions[i] + rot0*dist;
				var p1 = positions[i + 1] + rot1*dist;
				var v0 = ClosestPointOnALine(p0, t0.position, t1.position);
				var v1 = ClosestPointOnALine(p1, t0.position, t1.position);
				p0 = v0 + (p0 - v0).normalized * dist;
				p1 = v1 + (p1 - v1).normalized * dist;

				Draw.ingame.Line(p0, p1, Color.yellow);



				p0 = v0 + (p0 - v0).normalized * -dist;
				p1 = v1 + (p1 - v1).normalized * -dist;
				Draw.ingame.Line(positions[i], positions[i + 1], Color.white);
				Draw.ingame.Line(p0, p1, Color.white);

				pro0 = pro1;

				//v0 = ClosestPointOnALine(positions[i - 1], t0.position + up0 * 0.2f, t1.position + up1 * -0.2f);
				//v1 = ClosestPointOnALine(positions[i], t0.position + up0 * 0.2f, t1.position + up1 * -0.2f);

				//Draw.ingame.Line(v0, v1, Color.black);
			}
		}
	}
	Vector3 ClosestPointOnALine(Vector3 p, Vector3 a, Vector3 b)
	{
		var v = a + Vector3.Project(p - a, b - a);
		if (Vector3.Distance(v, b) > Vector3.Distance(a, b))
			return a;
		if (Vector3.Distance(v, a) > Vector3.Distance(a, b))
			return b;
		return v;
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	void DisplayCatmullRomSpline(int pos)
	{
		//The 4 points we need to form a spline between p1 and p2
		Vector3 p0 = points[ClampListPos(pos - 1)].position;
		Vector3 p1 = points[pos].position;
		Vector3 p2 = points[ClampListPos(pos + 1)].position;
		Vector3 p3 = points[ClampListPos(pos + 2)].position;

		//The start position of the line
		Vector3 lastPos = p1;
		positions.Add(lastPos);
		pointsDistance.Add(0);
		startInds.Add(pos);
		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 1.0f/reso;

		//How many times should we loop?
		int loops = Mathf.FloorToInt(reso);

		for (int i = 1; i <= loops; i++)
		{
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
			positions.Add(newPos);
			startInds.Add(pos);
			pointsDistance[pos] += Vector3.Distance(lastPos,newPos);
			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
		pointsDistance[pos] += Vector3.Distance(positions[positions.Count-1], p2);

	}

	//Clamp the list positions to allow looping
	int ClampListPos(int pos)
	{
		if (pos < 0)
		{
			pos = points.Count - 1;
		}

		if (pos > points.Count)
		{
			pos = 1;
		}
		else if (pos > points.Count - 1)
		{
			pos = 0;
		}

		return pos;
	}

	//Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
	//http://www.iquilezles.org/www/articles/minispline/minispline.htm
	Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

	//Display without having to press play
	void UpdateSpline()
	{
		points = new List<Transform>();
		pointsDistance = new List<float>();
		for (int i = 0; i < transform.childCount; i++)
		{
			points.Add(transform.GetChild(i));
		}


		positions = new List<Vector3>();
		startInds = new List<int>();
		//Draw the Catmull-Rom spline between the points
		for (int i = 0; i < points.Count-1; i++)
		{
			////Cant draw between the endpoints
			////Neither do we need to draw from the second to the last endpoint
			////...if we are not making a looping line
			//if ((i == 0 || i == points.Count - 2 || i == points.Count - 1) && !isLooping)
			//{
			//	continue;
			//}

			DisplayCatmullRomSpline(i);
		}
	}

	private void OnDrawGizmos()
	{
		Transform t;
		Vector3 up, rot;
		for(int i = 0; i < transform.childCount; i++)
		{
			//Gizmos.color = Color.white;
			//t = transform.GetChild(i);
			//Gizmos.DrawSphere(t.position, 0.05f);

			//Gizmos.color = Color.red;
			//up = t.localToWorldMatrix.MultiplyPoint3x4(Vector3.up) - t.position;
			//Gizmos.DrawSphere(t.position + up * 0.2f, 0.02f);
		}
		if (lockRotation)
		{
			t = transform.GetChild(transform.childCount - 1);
			float z = t.rotation.eulerAngles.z;
			up = t.localToWorldMatrix.MultiplyPoint3x4(Vector3.up) - t.position;
			t.LookAt(transform.GetChild(transform.childCount - 2), up);
			rot = t.rotation.eulerAngles;
			t.rotation = Quaternion.Euler(rot.x, rot.y, z);

			for (int i = 0; i < transform.childCount - 1; i++)
			{
				t = transform.GetChild(i);
				z = t.rotation.eulerAngles.z;
				up = t.localToWorldMatrix.MultiplyPoint3x4(Vector3.up) - t.position;
				t.LookAt(transform.GetChild(i + 1), up);
				rot = t.rotation.eulerAngles;
				t.rotation = Quaternion.Euler(rot.x, rot.y, z);
			}
		}
	
		
	}
}
