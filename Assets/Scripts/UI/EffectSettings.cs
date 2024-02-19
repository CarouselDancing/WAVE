using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EffectSettings : MonoBehaviour
{
    public XRInteractorLineVisual line;
    public SetID setId;
    public ToggleButton toggle;
    // Start is called before the first frame update

    private void OnEnable()
    {
        setId.enbleLine(true);
    }

    private void OnDisable()
    {
        setId.enbleLine(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = true;
    }
}
