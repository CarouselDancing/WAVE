using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorControl : MonoBehaviour
{
    public float speedTreshold = 0.1f;
    [Range(0,1)] public float smoothing = 1;
    Animator animator;
    Vector3 previousPos;
    VRrig vrRig;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRrig>();
        previousPos = vrRig.head.vrTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        var headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;
        var localSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        float previousX = animator.GetFloat("Xdirection");
        float previousY = animator.GetFloat("Ydirection");


        animator.SetBool("moving", localSpeed.magnitude > speedTreshold);
        animator.SetFloat("Xdirection", Mathf.Lerp(previousX, Mathf.Clamp(localSpeed.x, -1, 1),smoothing));
        animator.SetFloat("Ydirection", Mathf.Lerp(previousY, Mathf.Clamp(localSpeed.z, -1, 1),smoothing));

    }
}
