using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRays : MonoBehaviour
{
    [SerializeField] DrawRaysInCircle rays;
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    public Color leftC;
    public Color rightC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rays.DrawRays(left.position,leftC);
        rays.DrawRays(right.position, rightC);

    }
}
