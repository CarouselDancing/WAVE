using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMov : MonoBehaviour
{
    public Transform head;
    public Vector3 posOffset;
    public Vector3 rotOffset;
    public float Yoffset;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position + Vector3.left;
        //transform.rotation = target.rotation;
        var y = head.position.y + Yoffset;
        var v = head.position + (-head.forward * distance); //target.TransformPoint(posOffset);

        transform.position = new Vector3(v.x,y,v.z);
        //transform.rotation = target.rotation * Quaternion.Euler(rotOffset);
    }
}
