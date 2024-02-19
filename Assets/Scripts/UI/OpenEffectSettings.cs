using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class OpenEffectSettings : MonoBehaviour
{
    public XRController controller1;
    public XRController controller2;

    public GameObject settings;

    public BeatPlayer bp;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool bpSeen = false;
    bool b2;
    // Update is called once per frame
    void Update()
    {
        if (!bpSeen && !bp.gameObject.activeSelf)
            return;

        bpSeen = true;

        bool bool1, bool2;
        if ((controller1.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool1) && bool1) ||
            (controller2.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool2) && bool2))
        {
            if (b2)
            {
                print("HOI");
                settings.SetActive(!settings.activeSelf);
            }

            b2 = false;
        }
        else
        {
            b2 = true;
        }
    }
}
