using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
using TMPro;

public class HeightUpdate : MonoBehaviour
{
    public PlayerInfo pI;
    public Transform eyeLevel;
    public CanvasGroup cg;
    public Collider button;
    public TMP_Text num;
    public Questionaire Q;
    public UseQuestionaire useQ;
    public BeatPlayer beat;
    // Start is called before the first frame update
    void OnEnable()
    {
        button.enabled = false;
        cg.alpha = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        using (Draw.WithLineWidth(1))
        {
            Draw.ingame.Line(transform.position, new Vector3(transform.position.x, 0.05f, transform.position.z));
            Draw.ingame.Line(transform.position, transform.position + Vector3.left * 0.15f);
            Draw.ingame.Line(transform.position, transform.position + Vector3.right * 0.15f);
            Draw.ingame.CircleXY(new Vector3(transform.position.x, 0.025f, transform.position.z), 0.025f);

        }

        if(pI.height == 0)
            num.text = "???";
        else
            num.text = transform.position.y.ToString("0.00") + "m";
    }

    public void setHeight()
    {
        print("YO");
        pI.height = eyeLevel.position.y;

        button.enabled = true;
        cg.DOFade(1, 0.3f);
        transform.DOKill();
        transform.DOMove(new Vector3(transform.position.x, pI.height, transform.position.z),0.4f);

        
    }
}
