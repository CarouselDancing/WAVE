using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CopyEnable : MonoBehaviour
{

    public  XRInteractorLineVisual mono;
    public XRInteractorLineVisual monoTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        monoTarget.enabled = mono.enabled;
        
    }
}
