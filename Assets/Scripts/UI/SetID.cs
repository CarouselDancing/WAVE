using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetID : MonoBehaviour
{
    public XRInteractorLineVisual line;
    public Transform idTexts;
    public int id;
    // Start is called before the first frame update
    void Awake()
    {
        line.enabled = true;
    }
    //private void OnDisable()
    //{
    //    line.enabled = false;
    //}
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleUp(Transform t)
    {
        t.DOScale(1.5f, 0.2f);
    }
    public void ScaleDown(Transform t)
    {
        t.DOScale(1f, 0.2f);
    }

    public void AddOne(TMP_Text t)
    {
        int i = int.Parse(t.text);
        i = (i + 1) % 9;
        t.text = "" + i;
        //var seq = DOTween.Sequence();
        //seq.Append();
        t.transform.DOKill();
        t.transform.localScale = Vector3.one;
        t.transform.DOShakeScale(0.3f);
    }

    public void MinusOne(TMP_Text t)
    {
        int i = int.Parse(t.text);
        i = (i - 1) < 0 ? 9 : i-1;
        t.text = "" + i;
        t.transform.DOKill();
        t.transform.localScale = Vector3.one;
        t.transform.DOShakeScale(0.3f);

    }

    public void Deactivate(GameObject g)
    {
        g.SetActive(false);
    }

    public void Activate(GameObject g)
    {
        g.SetActive(true);
        g.transform.DOKill();
        g.transform.localScale = Vector3.one;
        g.transform.DOShakeScale(0.3f);
    }

    public void setID()
    {
        id = 0;
        for(int i = 0; i < idTexts.childCount; i++)
        {
            id += int.Parse(idTexts.GetChild(i).GetComponent<TMP_Text>().text) * (int)Mathf.Pow(10, i);
        }

        GameObject.FindObjectOfType<PlayerInfo>().UpdateIDs(id);
    }

    public void enbleLine(bool b = true)
    {
        line.enabled = b;
    }
}
