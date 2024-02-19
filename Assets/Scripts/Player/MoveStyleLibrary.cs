using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SpawnEffect
{
    public string Name;
    public GameObject effect;
}


public class MoveStyleLibrary : MonoBehaviour
{
    [SerializeField] List<SpawnEffect> styleLibrary;
    public Dictionary<string, GameObject> styles;

   
    // Start is called before the first frame update
    void Start()
    {
        styles = new Dictionary<string, GameObject>();
        for (int i = 0; i < styleLibrary.Count; i++)
            styles[styleLibrary[i].Name] = styleLibrary[i].effect;

        
    }

}
