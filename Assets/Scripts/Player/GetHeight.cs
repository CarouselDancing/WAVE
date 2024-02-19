using Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GetHeight : MonoBehaviour
{
    public Transform head;
    public float yOffset;
    public Transform[] Characters;
    public Transform CharacterEyeLevel;
    float origHeight;

    public XRController controller;
    // Start is called before the first frame update
    void Start()
    {
        Matrix4x4 M = Characters[0].worldToLocalMatrix;
        origHeight = M.MultiplyPoint3x4(CharacterEyeLevel.position).y / Characters[0].localScale.x;

        float scale = 0;

        //scale = CharacterEyeLevel.position.y / origHeight;

        //foreach (var character in Characters)
        //    character.localScale = Vector3.one * scale;

        //t1 = CharacterEyeLevel.position + Vector3.forward;
        //t2 = new Vector3(CharacterEyeLevel.position.x, origHeight * scale, CharacterEyeLevel.position.z);

    }
    Vector3 t1, t2;
    // Update is called once per frame
    bool pressedOnce;
    void Update()
    {
        bool pressed;

        if (controller.inputDevice.TryGetFeatureValue( CommonUsages.primaryButton,out pressed) && pressed)// OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.S))
        {
            if (!pressedOnce)
            {
                pressedOnce = true;
                print("primaryButton pressed");
                print(head.localPosition);

                float scale = 0;

                scale = (head.localPosition.y + yOffset) / origHeight;

                foreach (var character in Characters)
                {
                    int z = character.localScale.z < 0 ? -1 : 1;
                    if (character.GetComponent<VRrig>())
                    {

                        character.localScale = new Vector3(scale, scale, scale * z);
                        character.GetComponent<VRrig>().UpdateOffset(scale);
                    }
                    else
                        character.localScale = new Vector3(1f / scale, 1f / scale, 1f / scale * z);

                }

                t1 = head.position;
                t2 = new Vector3(CharacterEyeLevel.position.x, origHeight * scale, CharacterEyeLevel.position.z);
            }
            
        }
        else
        {
            pressedOnce = false;
        }
        //Draw.ingame.Line(t1, t2);
    }
}
