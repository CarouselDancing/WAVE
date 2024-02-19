using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScaleFromHeight : MonoBehaviour
{
    [SerializeField] List<Transform> objects;

    [SerializeField] float maxHeight;
    float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        if (objects.Count > 0)
            maxScale = objects[0].localScale.x; 
    }

    // Update is called once per frame
    void Update()
    {
        float y = transform.position.y;
        for(int i = 0; i < objects.Count; i++)
        {
            float scale = Mathf.Lerp( maxScale,0f, Mathf.Abs(objects[i].position.y - y) / maxHeight);
            objects[i].localScale = Vector3.one * scale;
        }
    }
}
