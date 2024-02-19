using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeSkyColor : MonoBehaviour
{

    public Material sky;
    public Color top, middle, bottom;
    Color oldTop, oldMiddle, oldBottom;
    private void OnEnable()
    {
        oldTop = sky.GetColor("_TopColor");
        oldMiddle = sky.GetColor("_HorizonColor");
        oldBottom = sky.GetColor("_BottomColor");

        sky.DOColor(top, "_TopColor", 1f);
        sky.DOColor(middle, "_HorizonColor", 1f);
        sky.DOColor(bottom, "_BottomColor", 1f);
    }

    private void OnDisable()
    {
        sky.DOColor(oldTop, "_TopColor", 1f);
        sky.DOColor(oldMiddle, "_HorizonColor", 1f);
        sky.DOColor(oldBottom, "_BottomColor", 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
