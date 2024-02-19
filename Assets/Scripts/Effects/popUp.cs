using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUp : MonoBehaviour
{
    GameObject particleEffect;
    // Start is called before the first frame update
    private void OnEnable()
    {
        float scale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 0.3f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        var g = Instantiate(particleEffect);
        g.transform.position = transform.position;
        Destroy(g, 3);
    }
}
