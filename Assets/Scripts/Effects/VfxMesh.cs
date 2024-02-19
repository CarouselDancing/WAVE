using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxMesh : MonoBehaviour
{
    public SkinnedMeshRenderer SMRenderer;
    public VisualEffect vfx;
    public float refreshRate;
    public bool updateMesh = true;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(BakeSMR());
    }

    IEnumerator BakeSMR()
    {
        while (gameObject.activeSelf && updateMesh)
        {
            var mesh = new Mesh();
            SMRenderer.BakeMesh(mesh);
            var mesh2 = new Mesh();
            mesh2.vertices = mesh.vertices;
            vfx.SetMesh("Mesh", mesh2);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
