using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.Events;

public class SetValue : MonoBehaviour
{
    public XRController controller;
    public BeatPlayer beatplayer;
    public bool decimals = true;
    public int minValue;
    public int maxValue;
    public float addAmount;
    float timeUpdated;
    public float timeBetweenUpdates;

    public float value;
    public TMP_Text tmp;

    public bool useEvents = false;
    public UnityEvent events;

    public bool setMaterialValue;
    public Material material;
    public string materialValue;

    public bool updateBez = false;
    public Bezier bez;


    public Shader opaqueShader;
    public Shader transparentShader;

    // Start is called before the first frame update
    void Start()
    {
        transparentShader = material.shader;

        value = Mathf.Max(minValue, value);
        value = Mathf.Min(maxValue, value);

        if (decimals)
            tmp.text = value.ToString("#.00");
        else
            tmp.text = (int)value + "";

    }

    // Update is called once per frame
    void Update()
    {


        Vector2 v = Vector2.zero;
        var t = CommonUsages.primary2DAxis;

        if (controller.inputDevice.TryGetFeatureValue(t, out v) && Time.time - timeUpdated > timeBetweenUpdates && v.x != 0)
        {
            timeUpdated = Time.time;
            value += addAmount * Mathf.Sign(v.x);
            value = Mathf.Max(minValue, value);
            value = Mathf.Min(maxValue, value);
            if(decimals)
                tmp.text = value.ToString("#.00");
            else
                tmp.text = (int)value + "";
            if (setMaterialValue)
            {
                if (value == 1)
                    material.shader = opaqueShader;
                else
                    material.shader = transparentShader;

                material.SetFloat(materialValue, value);
            }
            if (useEvents)
                events.Invoke();
            if (updateBez)
                bez.UpdatePoint((int)value);
        }


    }

    public void SetBeatSpeed()
    {
        if (beatplayer.danceQ == null)
            return;

        beatplayer.beatsToTarget = (int) value;
        beatplayer.danceQ.UpdateAnimations();
    }
}
