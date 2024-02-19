using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 posOffset;
    public Vector3 rotOffset;
    public void Map(bool mirror = false)
    {
        rigTarget.position = vrTarget.TransformPoint(posOffset);
     
        if(!mirror)
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(rotOffset);
        else
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(rotOffset) * Quaternion.Euler(new Vector3(180, 0, 0));
    }
}

public class VRrig : MonoBehaviour
{

    public VRMap head;
    public VRMap RHand;
    public VRMap LHand;
    public bool mirror;
    public Transform constrains;

    public Transform headConstrain;
    public Vector3 headOffset;
    Vector3 offset;

    public bool moveWithHeadset;
    // Start is called before the first frame update
    void Start()
    {
        headOffset = transform.position - headConstrain.position;
        offset = headOffset;
    }

    public void UpdateOffset(float scale)
    {
        headOffset = offset * scale;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveWithHeadset)
        {
            transform.position = headConstrain.position + headOffset;
            transform.forward = Vector3.ProjectOnPlane(headConstrain.up, Vector3.up).normalized;
        }
        
        //if (mirror)
        //    transform.forward *= -1; 

        head.Map(mirror);
        LHand.Map(mirror);
        RHand.Map(mirror);
    }
}
