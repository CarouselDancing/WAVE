using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpCopyEnable : MonoBehaviour
{
    public SkinnedMeshRenderer master_mono;
    public MeshRenderer mono;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mono.enabled = master_mono.material.name[0] == 'f';
    }
}
