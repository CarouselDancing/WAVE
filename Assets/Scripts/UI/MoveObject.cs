using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.Events;
using Drawing;
public class MoveObject : MonoBehaviour
{
    public int index;
    public Transform hand;
    public Bezier bez;
    public XRController controller;
    public float speed = 5f;
    bool moveOK = false;

    public Material m0;
    public Material m1;

    public BeatPlayer bp;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(bp == null)
            bp = GameObject.FindObjectOfType<BeatPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveOK)
            return;

        Draw.ingame.Line(transform.position, transform.position + Vector3.down * transform.position.y, Color.yellow);

        Vector2 v = Vector2.zero;
        var t = CommonUsages.primary2DAxis;

        if (controller.inputDevice.TryGetFeatureValue(t, out v)  && v.x != 0)
        {
            float y = transform.position.y;

            var vv = Vector3.ProjectOnPlane(hand.forward, Vector3.up);
            var z = vv * v.y;

            vv = Vector3.ProjectOnPlane(hand.right, Vector3.up);

            var x = vv * v.x;
            x = (x + z);
            x = new Vector3(x.x, 0, x.z);
            transform.position += x * speed * Time.deltaTime;
            if (transform.position.magnitude > 12)
                transform.position =  transform.position.normalized * 12;

            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if (!bp.gameObject.activeSelf || bp.danceQ == null)
                bez.updatePoints();
            else
                bp.danceQ.UpdatePositions(index);
        }
    }

    public void ToggleMove()
    {
        moveOK = !moveOK;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = moveOK ? m1 : m0;
    }
}
