using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;

public class StartLevel : MonoBehaviour
{
    public AudioSource ambient;
    PlayerInfo pInfo;
    public BeatPlayer bp;
    GameObject currentLevel;
    public XRInteractorLineVisual line;
    public CanvasGroup GoBackText;
    public CanvasGroup startText;
    public Collider startBox;
    public Transform head;
    public VisualEffect circle;
    public float range = 0.5f;
    [SerializeField] Material floorMaterial;
    //GameObject currentLevel;
    private void OnEnable()
    {
        line.enabled = true;
        pInfo = GameObject.FindObjectOfType<PlayerInfo>();
        circle.SetFloat("R", range);


        int ind = pInfo.currentLevelIndex;
        ind++;
        if (ind > pInfo.Levels.Count - 1)
        {
            ind = 0;
        }

        currentLevel = Instantiate(pInfo.Levels[ind % pInfo.Levels.Count]);


        transform.position = currentLevel.GetComponent<LevelInfo>().PlayerOffset;

    }

    private void OnDisable()
    {
        line.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    bool isCloseEnough;
    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(head.position.x, head.position.z));
        if(dist < range && !isCloseEnough)
        {
            GoBackText.DOFade(0, 0.4f);
            startText.DOFade(1, 0.4f);
            //startText.transform.DOShakeScale(0.4f);
            startBox.enabled = true;
            circle.SetBool("rateZero", true);
        }
        else if (dist > range && isCloseEnough)
        {
            GoBackText.DOFade(1, 0.4f);
            startText.DOFade(0.2f, 0.4f);
            startBox.enabled = false;
            circle.SetBool("rateZero", false);

        }

        isCloseEnough = dist > range;
    }


    public void StartNewLevel()
    {
        gameObject.SetActive(false);
        floorMaterial.DOKill();
        ambient.DOKill();
        ambient.DOFade(0, 4);
        var seq = DOTween.Sequence();
        seq.Append( floorMaterial.DOFloat(22f, "_Zvalue", 7));
        seq.OnComplete(() => 
        {
          
            bp.danceQ = null;


            if (pInfo.hasPlayedTutorial)
                pInfo.currentLevelIndex++;
            if (pInfo.currentLevelIndex > pInfo.Levels.Count - 1)
            {
                pInfo.currentLevelIndex = 0;
                pInfo.stylesPlayed++;
            }
            pInfo.levelsPlayed++;

            //if (pInfo.hasPlayedTutorial)
            //{
            //    currentLevel = Instantiate(pInfo.Levels[pInfo.currentLevelIndex % pInfo.Levels.Count]);

            //}
            //else
            //{
            //    currentLevel = Instantiate(pInfo.tutorial);

            //}

            //transform.position = currentLevel.GetComponent<LevelInfo>().PlayerOffset;


            _StartNewLevel(currentLevel);

        });

        
    }
      
    public void _StartNewLevel(GameObject currentLevel)
    {
        

        var levelInfo = currentLevel.GetComponent<LevelInfo>();

        //currentLevel.transform.localScale = Vector3.one * (pInfo.height / levelInfo.eyeLevel);

        bp.director = levelInfo.director;
        bp.beatCount = levelInfo.beatDivide;
        bp.levelInfo = levelInfo;

        var mode = pInfo.playStyles[0];
        //var mode = pInfo.hasPlayedTutorial ?
        //    pInfo.playStyles[(pInfo.ID + pInfo.stylesPlayed) % pInfo.playStyles.Count] :
        //    pInfo.tutorialGameMode;

        bp.teacherScale = mode.teacherScale;
        currentLevel.transform.GetChild(1).GetChild(0).localScale = Vector3.one * mode.teacherScale;

        bp.showPartners = mode.showPartners;
        bp.showQ = mode.showQ;
        bp.showTeacher = mode.showTeacher;
        bp.realTime = mode.realTime;
        bp.showLine = mode.showLine;
        bp.mode = mode;
        bp.gameObject.SetActive(true);
        currentLevel.SetActive(true);

        //pInfo.nextGameMode();
      
        pInfo.hasPlayedTutorial = true;

       
    }

    
}


