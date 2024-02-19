using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SavePerformanceAsSobj : MonoBehaviour
{
    public TextAsset json;
    Performance p;
    PerformanceSobj pSobj;
    
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (json != null)
        {
            p = JsonUtility.FromJson<Performance>(json.ToString());
            pSobj = ScriptableObject.CreateInstance<PerformanceSobj>();
            if (name == "")
                name = "performance";
            AssetDatabase.CreateAsset(pSobj, "Assets/Resources/Performances/" + p.name + ".asset");



            pSobj.extraPointsBetweenBeats = p.extraPointsBetweenBeats;
            pSobj.Name = p.name;
            pSobj.originalPlayerHeight = p.originalPlayerHeight;
            pSobj.Lhand = p.Lhand;
            pSobj.Rhand = p.Rhand;
            pSobj.head = p.head;
            pSobj.startTime = p.startTime;
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(pSobj);
            AssetDatabase.SaveAssets();

        }
#endif

    }

}
