using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ChangeFormation : MonoBehaviour
{
    public Color startColor;
    public Color endColor;

    public XRController controller1;
    public XRController controller2;

    public BeatPlayer beat;
    public List<MessyPreset> presets;
    public MessyPreset preset;
    public int index = 0;
    public CanvasGroup canvas;
    public TMP_Text tmp;
    public int maxIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    bool bool1 = false;
    // Update is called once per frame
    //void Update()
    //{
    //    bool pressed1, pressed2;
    //    if ((controller1.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out pressed1) && pressed1) ||
    //        (controller2.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out pressed2) && pressed2))
    //    {
    //        if (!bool1)
    //        {
    //            changePreset();
    //        }
    //        bool1 = true;
    //    }
    //    else
    //        bool1 = false;
    //}

    Sequence seq;
    public void changePreset()
    {
        if (!beat.gameObject.activeSelf)
            return;

        preset = presets[index];

        preset.OnEnable();

        tmp.text = "" + preset.formationName;
        //canvas.DOKill();
        //seq.Kill();
        //canvas.alpha = 1;
        //seq = DOTween.Sequence();
        //seq.Append( canvas.DOFade(0, 0.5f));
        //seq.PrependInterval(1.5f);
        
        
        index = (index + 1) % maxIndex; //HOX! SHOULD BE THIS
        index = 1;
    }
}
