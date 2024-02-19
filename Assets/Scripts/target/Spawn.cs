using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject effect;
  
    public void SpawnEffect()
    {

        var g = Instantiate(effect);
        g.transform.position = transform.position;
    }
}
