using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FeetCirclePos : MonoBehaviour
{
    public RayCircle rc;
    public Transform foot0;
    public Transform foot1;
    public float y;
    public VisualEffect circle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = foot0.position + (foot1.position-foot0.position)/2;
        transform.position = new Vector3(pos.x, y, pos.z);
        float R = Vector3.Distance(foot0.position, foot1.position);

        rc.R = Mathf.Max(0.5f, R);

        circle.SetFloat("R", Mathf.Max(0.5f, R));

    }
}
