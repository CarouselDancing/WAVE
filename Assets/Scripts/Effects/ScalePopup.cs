using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePopup : MonoBehaviour
{
    Vector3 scale;
    LifeTime life;

    private void Awake()
    {
        life = GetComponent<LifeTime>();
    }
    private void OnEnable()
    {
        if(scale == Vector3.zero)
            scale = transform.localScale;

        if(life.lifeTime <= 0)
            life.lifeTime = 1;
          
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 0.2f);
    }

   

    // Update is called once per frame
    void Update()
    {
        if(life.lifeTime < 0.2)
            transform.DOScale(0, 0.2f);
    }
}
