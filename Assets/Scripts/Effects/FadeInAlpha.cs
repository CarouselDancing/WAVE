using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FadeInAlpha : MonoBehaviour
{
    Material m;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnEnable()
    {
        m = gameObject.GetComponent<SkinnedMeshRenderer>().material;
        m.DOKill();
        m.SetFloat("master_alpha", 0);
        m.DOFloat(1, "master_alpha", 5f);
    }
}
