using Drawing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using JetBrains.Annotations;
using System;

public class UseQuestionaire : MonoBehaviour
{
    public GameObject tTargetR;
    public GameObject tTargetL;

    public PlaySound sound;
    public AudioClip clip;
    public LayerMask mask;
    Transform head;
    public string fileName = "testQuestionare";
    public LayerMask layerMask;
    int[] results;
    public XRInteractorLineVisual line;
    public Questionaire questionaire;
    public float lineLenght;
    public float lineWidth;
    public float YOffset;
    [Space]
    public int questionIndex, trueQueationIndex;
    public TMP_Text numberVisual;
    public Transform startText;
    public Transform endText;
    public TMP_Text info;
    public TMP_Text page;
    public TMP_Text doneText;
    public TMP_Text userAnswer;
    public Transform slider;
    public Transform interactable;
    public GameObject next, previous, nextVisual, prevVisual, done;
    Vector3 startPoint, endPoint;
    Vector3[] points;
    int pointIndex;
    // Start is called before the first frame update
    bool updateSlider;

    int[] qOrder;
    private void OnEnable()
    {
        //Debug.Log(Application.persistentDataPath);
        if (questionaire == null)
        {
            print("END");
            GameObject.FindObjectOfType<PlayerInfo>().EndQuestionaire();
            return;
        }

        print("HERE");


        qOrder = new int[questionaire.questions.Count];
        for (int i = 0; i < qOrder.Length; i++)
            qOrder[i] = i;

        if (questionaire.randomOrder)
            Shuffle();


        head = Camera.main.transform;
        info.transform.localScale = Vector3.one;

        trueQueationIndex = 0;
        questionIndex = qOrder[trueQueationIndex];

        done.SetActive(false);
        line.enabled = true;
        results = new int[questionaire.questions.Count];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = questionaire.questions[i].numberOfAnswers / 2;
        }
        selected = results[questionIndex];
        //numberVisual.text = "" + 50;// questionaire.questions[questionIndex].showNumber ? "" + (selected + questionaire.questions[questionIndex].firstNumber) : "";
        previous.SetActive(false);
        next.SetActive(true);
        prevVisual.GetComponent<CanvasGroup>().DOFade(0.1f, 0.3f);
        nextVisual.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        updatePoints();
        updateQuestion();

        doneText.text = questionaire.done;
    }

    void Shuffle()
    {
        

        var count = qOrder.Length;
        if (!questionaire.lastRandom)
            count--;
        
        int startInd = !questionaire.firstRandom ? 1 : 0;
        var last = count - 1;

        for (var i = startInd; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = qOrder[i];
            qOrder[i] = qOrder[r];
            qOrder[r] = tmp;
        }
    }
    public void setQuestionaire(Questionaire q)
    {
        questionaire = q;
    }
    //private void OnDisable()
    //{
    //    line.enabled = false;
    //}

    Vector3 toW(Vector3 v)
    {
        var vv = transform.localToWorldMatrix.MultiplyPoint3x4(v);
        return vv;
    }

    Vector3 toL(Vector3 v)
    {
        var vv = transform.worldToLocalMatrix.MultiplyPoint3x4(v);
        return vv;
    }
    void Start()
    {
        
    }
    int selected = 0;

    float timer;
    // Update is called once per frame
    void Update()
    {
        updatePoints();
        RaycastHit hit1;
        if (Physics.Raycast(head.position, head.forward, out hit1, 100, mask))
        {

            Debug.DrawLine(head.position, hit1.point, Color.red);
            timer = 0;
        }
        else if (!updateSlider && timer > 1)
        {
            Debug.DrawLine(head.position, head.position + head.forward*100, Color.yellow);

            Vector3 v = head.position + head.forward * 1.5f;
            v.y = Mathf.Max(1, head.position.y);
            v = (v - head.position).normalized * 1.8f;
            transform.DORotate(new Vector3(0, head.rotation.eulerAngles.y, 0), 1);// DOLookAt(head.position + v * 2, 0);
            transform.DOMove(head.position + v, 1);


            timer = 0;
        }
        else if (!updateSlider)
        {
            timer += Time.deltaTime;
        }
        else
            timer = 0;

        if(!questionaire.questions[questionIndex].onlyText)
        {
            using (Draw.ingame.WithLineWidth(lineWidth))
            {
                Draw.ingame.Line(toW(startPoint),toW( endPoint));

                foreach (var v in points)
                {
                     var vv = toW(v);
                    Draw.ingame.Line(vv + Vector3.up * 0.02f, vv - Vector3.up * 0.02f);

                }
            }
        }
       

        if (updateSlider)
        {
            RaycastHit hit;
            if(Physics.Raycast(interactable.position, interactable.forward,out hit,100f, layerMask))
            {

                int oldS = selected;
                for(int i = 0; i < points.Length; i++)
                {
                    float d = Vector3.Distance(toW( points[i]), hit.point);
                    if (d < Vector3.Distance(toW(points[selected]), hit.point))
                        selected = i;
                }

                if(oldS != selected)
                {
                    slider.DOMove(toW(points[selected]), 0.1f);
                    sound._PlaySound(clip);
                    numberVisual.text = questionaire.questions[questionIndex].showNumber ? "" + (selected + questionaire.questions[questionIndex].firstNumber) : "";
                }
              
            }

            if (questionaire.questions[questionIndex].answers.Count > results[questionIndex])
            {
                userAnswer.text = "" + questionaire.questions[questionIndex].answers[selected];
            }
            else
                userAnswer.text = "";
        }
       

    }

    void updatePoints()
    {
        startPoint = transform.position - transform.right * lineLenght / 2 - Vector3.down * YOffset;
        endPoint = transform.position + transform.right * lineLenght / 2 - Vector3.down * YOffset;
        endText.transform.position = endPoint;
        startText.transform.position = startPoint;

        startPoint = toL(startPoint);
        endPoint = toL(endPoint);
    }

    void updateQuestion()
    {
        
        var Q = questionaire.questions[questionIndex];
        //////
        if (Q.effect == "showTarget")
        {
            tTargetR.SetActive(true);
            tTargetL.SetActive(true);

        }
        else if (Q.effect == "hideTarget")
        {
            tTargetR.SetActive(false);
            tTargetL.SetActive(false);
        }

        /////
        info.text = Q.question;
        info.transform.DOKill();
        info.transform.localScale = Vector3.one;
        info.transform.DOShakeScale(0.3f);
        points = new Vector3[Q.numberOfAnswers];

        page.text = (trueQueationIndex+1) + "/" + questionaire.questions.Count;

        


        if (Q.onlyText) //points.Length < 2)
        {
            slider.gameObject.SetActive(false);
            interactable.gameObject.SetActive(false);

            numberVisual.text = "";
            startText.GetChild(0).GetComponent<TMP_Text>().text = "";
            endText.GetChild(0).GetComponent<TMP_Text>().text = "";

            userAnswer.text = "";
        }
        else
        {
            startText.GetChild(0).GetComponent<TMP_Text>().text = Q.startText;
            endText.GetChild(0).GetComponent<TMP_Text>().text = Q.endText;

            slider.gameObject.SetActive(true);
            interactable.gameObject.SetActive(true);

            for (int i = 0; i < Q.numberOfAnswers; i++)
            {
                points[i] = startPoint + Vector3.right * lineLenght / (Q.numberOfAnswers - 1) * i;
            }

            slider.position = toW( points[results[questionIndex]] );
            interactable.position = toW(points[results[questionIndex]]);


            numberVisual.text = questionaire.questions[questionIndex].showNumber ? "" + (selected + questionaire.questions[questionIndex].firstNumber) : ""; //Q.showNumber ? "" + (results[questionIndex]) : ""; //

            if (questionaire.questions[questionIndex].answers.Count > results[questionIndex] )
            {
                userAnswer.text = "" + questionaire.questions[questionIndex].answers[results[questionIndex] ];
            }
            else
                userAnswer.text = "";
        }
        
    }
    public void grabSlider()
    {
        if (updateSlider)
            return;
        slider.DOScale(1.5f,0.2f);
        updateSlider = true;
    }

    public void dropSlider()
    {
        slider.DOScale(1f, 0.2f);
        updateSlider = false;
        interactable.DOMove(toW( points[selected]), 0.2f);
    }

    public void PreviousQ()
    {
        results[questionIndex] = selected;
        trueQueationIndex--;
        questionIndex = qOrder[trueQueationIndex];
        selected = results[questionIndex];
        updatePoints();
        updateQuestion();
        if (trueQueationIndex == 0)
        {
            prevVisual.GetComponent<CanvasGroup>().DOFade(0.1f, 0.3f);
            previous.SetActive(false);
        }
        nextVisual.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        next.SetActive(true);
        done.SetActive(false);
    }

    public void NextQ()
    {
        results[questionIndex] = selected;
        trueQueationIndex++;
        questionIndex = qOrder[trueQueationIndex];
        selected = results[questionIndex];// - questionaire.questions[questionIndex].firstNumber;
        updatePoints();
        updateQuestion();

        if (trueQueationIndex == results.Length - 1)
        {
            nextVisual.GetComponent<CanvasGroup>().DOFade(0.1f, 0.3f);
            next.SetActive(false);
            if(questionaire.ID != "END")
                done.SetActive(true);
        }
        prevVisual.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        previous.SetActive(true);


    }

    public void saveQuestionareResults()
    {

        done.transform.DOShakeScale(0.3f);
        List<Result> finalResults = new List<Result>();
        for(int i = 0; i <results.Length; i++)
        {
            if (!questionaire.questions[i].onlyText)
            {
                var r = new Result();
                r.answer = results[i] + questionaire.questions[i].firstNumber;
                r.question = questionaire.questions[i];
                finalResults.Add(r);
            }
                
        }
        if (finalResults.Count == 0)
            return;
        var pInfo = GameObject.FindObjectOfType<PlayerInfo>();
        var Q = new questionaireResults();
        Q.questionaire_ID = questionaire.ID;
        var info = GameObject.FindObjectOfType<PlayerInfo>();
        Q.player_ID = info.ID;
        Q.results = finalResults;
        string json = JsonConvert.SerializeObject(Q); //JsonUtility.ToJson(results);
        string fileName = "Q_" +((pInfo.ID + pInfo.stylesPlayed) % pInfo.playStyles.Count) + "_" + GameObject.FindObjectOfType<ChangeFormation>().preset.formationName; //; // pInfo.modeQDone ?  : "Q_" + questionaire.ID;
        var path = pInfo.GetDirectory() + "/"  + fileName + ".json";
        print(json);
        print(path);
        File.WriteAllText(path,json);
    }

    public void scaleUp(Transform t)
    {
        t.DOScaleX(1.5f, 0.2f);
        t.DOScaleY(1.5f, 0.2f);
    }
    public void scaleNormal(Transform t)
    {
        t.DOScaleX(1f, 0.2f);
        t.DOScaleY(1f, 0.2f);
    }
}

public class questionaireResults
{
    public string questionaire_ID;
    public int player_ID;
    public List<Result> results;
}
[Serializable]
public struct Result
{
    public Question question;
    public int answer;
}
