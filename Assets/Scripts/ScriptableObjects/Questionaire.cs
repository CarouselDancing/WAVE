using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question 
{
    [TextArea] public string question;
    public string effect;
    public bool onlyText = true;
    public bool showNumber = true;
    public int firstNumber;
    [Range(1,101)] public int numberOfAnswers;
    public bool showAnswer;
    public List<string> answers;
    public string startText;
    public string endText;
    
}

[CreateAssetMenu(fileName = "questionaire", menuName = "ScriptableObjects/questionaire", order = 1)]
public class Questionaire : ScriptableObject
{
    public string ID;
    public bool randomOrder;
    public bool firstRandom;
    public bool lastRandom;


    public string done = "DONE";
    public List<Question> questions;

    //public Questionaire() { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
