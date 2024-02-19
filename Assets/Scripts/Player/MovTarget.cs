using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTarget : MonoBehaviour
{
    public Material normM;
    public Material hitM;
    MeshRenderer mr;
    public string pairName;
    public bool inside;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == pairName)
        {
            inside = true;
            mr.material = hitM;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == pairName)
        {
            inside = false;
            mr.material = normM;
        }

    }

}
