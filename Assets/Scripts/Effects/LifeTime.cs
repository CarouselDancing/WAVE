using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float lifeTime;
    public bool count;
 

    // Update is called once per frame
    void Update()
    {
        if(count)
            lifeTime -= Time.deltaTime;
    }
}
