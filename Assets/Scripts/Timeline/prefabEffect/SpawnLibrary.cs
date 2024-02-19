using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLibrary : MonoBehaviour
{
    public List<SpawnEffect> prefabs;
    Dictionary<string, GameObject> library;
    // Start is called before the first frame update
    void Start()
    {
        library = new Dictionary<string, GameObject>();
        foreach (var i in prefabs)
            library[i.Name] = i.effect;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

  
    public GameObject GetObject(string id)
    {
        return library[id];
    }
}
