using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public GameObject Q_UI;
    public GameObject S_UI;
    public GameObject H_UI;
    
    public string directoryName = "Test";
    public Questionaire endQuestionaire;
    public Questionaire modeQuestionaire;
    public int ID;
    public GameObject tutorial;
    public List<GameObject> Levels;
    //public int howManyStyles;
    public int stylesPlayed;
    //public int currentPlayStyleIndex = -1;
    public int currentLevelIndex = -1;
    public int levelsPlayed;
    public bool modeQDone = true;
    public GameMode tutorialGameMode;
    public List<GameMode> playStyles;
    List<int> levelOrder;
    public bool hasPlayedTutorial;

    public float height = 0;

    public ChangeFormation formation;
    public void UpdateIDs(int id)
    {
        directoryName = System.DateTime.UtcNow.ToString("yyyy-MMMM-dd_HH_mm") + "_ID-" + id;
        Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
        print(Application.persistentDataPath + "/" + directoryName);
        ID = (int)(id);// / 10);
        formation.index = ID % formation.maxIndex;
        formation.preset = formation.presets[formation.index];

        //levelID = ids - (ID * 10);
        //currentPlayStyleIndex = ID % playStyles.Count;
    }

    public void CreateLevelDirectory()
    {
        print(GameObject.FindObjectOfType<ChangeFormation>().preset.formationName);

        Directory.CreateDirectory(GetDirectory());
    }
    public string GetDirectory()
    {
        //if (modeQDone)
        //    return Application.persistentDataPath + "/" + directoryName;
        return Application.persistentDataPath + "/" + directoryName + "/" + "mode-" + ((ID + stylesPlayed) % playStyles.Count) + "_"+ "music-" + currentLevelIndex + "_" + GameObject.FindObjectOfType<ChangeFormation>().preset.formationName;
    }
    //public void nextGameMode()
    //{
    //    currentPlayStyleIndex = (currentPlayStyleIndex+1) % playStyles.Count;
    //}

    [Serializable]
    public class GameMode
    {
        public string Name;
        public Questionaire questions;
        public bool showTeacher, showPartners, realTime, showQ, showLine;
        public int teacherScale = 1;
    }


    public void EndQuestionaire()
    {
        Q_UI.SetActive(false);
        print(modeQDone);
        if (!modeQDone)//levelsPlayed % Levels.Count == 0 && 
        {
            modeQDone = true;
            var q = Q_UI.GetComponent<UseQuestionaire>();
            q.questionaire = modeQuestionaire;
            Q_UI.SetActive(true);
            return;
        }
        modeQDone = false;

        //HOX! THIS SHOULD NOT BE COMMENTED
        /*
        if (currentLevelIndex == (Levels.Count - 1) && stylesPlayed == (playStyles.Count - 1)) //howManyStyles <= stylesPlayed)
        {
            var q = Q_UI.GetComponent<UseQuestionaire>();
            if (q.questionaire.ID == endQuestionaire.ID)
            {
                return;
            }
            else
            {
                q.questionaire = endQuestionaire;
                Q_UI.SetActive(true);
            }
        }
        else
        */
        {
            //S_UI.SetActive(true); // HOX! SHOULD BE THIS ONE
            H_UI.SetActive(true); 
        }
    }
}
