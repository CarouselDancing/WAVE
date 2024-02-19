using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotationXYZ : MonoBehaviour
{
    public Transform target;

    public bool copyX;
    public bool copyY;
    public bool copyZ;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    [Range(0,2)] public int copyXTo;
    [Range(0, 2)] public int copyYTo;
    [Range(0, 2)] public int copyZTo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rot = target.rotation.eulerAngles;

        if (copyX)
            CopyRot(rot.x, offsetX, copyXTo);
        if (copyY)
            CopyRot(rot.y, offsetY, copyYTo);
        if (copyZ)
            CopyRot(rot.z, offsetZ, copyZTo);
    }

    void CopyRot(float targetRot, float offset, int index)
    {
        var rot = targetRot + offset;
        var angl = transform.rotation.eulerAngles;
        angl[index] = rot;
        transform.rotation = Quaternion.Euler(angl);
    }
}
