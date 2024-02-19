using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FinalJson : MonoBehaviour
{
    public string path;
    public string MovementJsons;
    public string saveDir;
    // Start is called before the first frame update
    void Start()
    {
        print(Application.persistentDataPath);
        CreateAllTesterJsons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CreateAllTesterJsons()
    {
        print("start");
        var allTesters = new List<Tester>();

        var folders = Directory.GetDirectories(path);
        print(folders.Length);
        foreach (var folder in folders)
        {
            CreateTesterJson(folder, allTesters);
        }

        string json = JsonConvert.SerializeObject(allTesters);
        File.WriteAllText(saveDir + "/UsertestData.json", json);

    }
    void CreateTesterJson(string testerPath, List<Tester> allTesters)
    {
        var N = testerPath.Split('-');
        string id = N[N.Length - 1];
        print(id);
        Tester tester = new Tester(int.Parse(id));

        var folders = Directory.GetDirectories(testerPath);
        int modeNum = 0;
        foreach (var folder in folders)
        {
            if (folder[folder.Length - 1] == '0')
                continue;

            string Q_json = File.ReadAllText(folder + "/Q_" + modeNum + ".json");
            questionaireResults Q = JsonUtility.FromJson<questionaireResults>(Q_json);
            print(Q.results.Count);
            modeNum++;
            print(folder);

            int sum = 0;
            for(int i = 0; i < 3; i++)
            {
                sum += Q.results[i].answer;
            }
            tester.PXI_M.Add(Math.Round(sum / 6.0,3));
            
            sum = 0;

            for (int i = 3; i < 6; i++)
            {
                sum += Q.results[i].answer;
            }
            tester.PXI_I.Add(Math.Round(sum / 6.0, 3));
            sum = 0;

            for (int i = 6; i < 13; i++)
            {
                sum += Q.results[i].answer;

            }
            tester.IMI.Add(Math.Round(sum / 7.0,3));

            tester.danceFeel.Add(Q.results[13].answer);

        }

        for(int m=0; m < 3; m++)
        {
            var mode = new List<float>();
            for(int d=0; d < 2; d++)
            {
                string movData = File.ReadAllText(MovementJsons + "/"+ id + "_" + m + "_" + d + ".json");
                simpleMovJson sj = JsonUtility.FromJson<simpleMovJson>(movData);
                print(sj.Name + " " + sj.Subjects[0].Positions[0].X);

                float meanDist = 0;
                Vector3 average = averagePos(sj, new List<string>() { "head", "R_hand", "L_hand" });
                Vector3 t_average = averagePos(sj, new List<string>() { "t_head", "t_R_hand", "t_L_hand" });

                meanDist += GetDistancesFromTarget(sj, "head", average, t_average);
                meanDist += GetDistancesFromTarget(sj, "R_hand", average, t_average);
                meanDist += GetDistancesFromTarget(sj, "L_hand", average, t_average);

                mode.Add(meanDist / 3f);
               
            }
            tester.modes.Add(mode);
        }

        allTesters.Add(tester);
    }

    Vector3 averagePos(simpleMovJson data, List<string> subjects)
    {
        List<Position> positions = new List<Position>();

        foreach (string s in subjects)
        {
            positions.AddRange(data.Subjects.Find(d => d.Name == s).Positions);
        }

        int count = positions.Count;

        Position sum = new Position();

        for(int i = 0; i < positions.Count; i++)
        {
            sum.X += positions[i].X;
            sum.Y += positions[i].Y;
            sum.Z += positions[i].Z;

        }

        sum.X = sum.X / count;
        sum.Y = sum.Y / count;
        sum.Z = sum.Z / count;

        return new Vector3(sum.X,sum.Y,sum.Z);
    }

    float GetDistancesFromTarget( simpleMovJson data, string subjectName, Vector3 average, Vector3 average_t)
    {
        var subject =  data.Subjects.Find(d => d.Name == subjectName);
        var subject_t = data.Subjects.Find(d => d.Name == "t_" + subjectName);

        float results = 0;

        int count = Mathf.Min(subject.Positions.Count, subject_t.Positions.Count);
        float startTime = subject.Positions[0].Time;
        float endTime = subject.Positions[subject.Positions.Count-1].Time;

        for (int i = 0; i < count; i++)
        {
            if (subject.Positions[i].Time - startTime < 10)
                continue;
            if (endTime - subject.Positions[i].Time < 10)
                break;

            Position p = subject.Positions[i];
            Vector3 v = new Vector3(p.X, p.Y, p.Z);
            v = -average + v;

            Position p_t = subject_t.Positions[i];
            Vector3 v_t = new Vector3(p_t.X, p_t.Y, p_t.Z);
            v_t = -average_t + v_t;

            results += Vector3.Distance(v, v_t); //Vector3.Distance(v, average) - Vector3.Distance(v_t, average_t); // Vector3.Distance(v, v_t);
        }

        return results /  (float) count;
    }

    [Serializable]
    public class simpleMovJson
    {
        public string Name;
        public List<MovData> Subjects;
    }
    [Serializable]
    public struct MovData
    {
        public string Name;
        public List<Position> Positions;
    }

    [Serializable]
    public struct Position
    {
        public float Time;
        public float X;
        public float Y;
        public float Z;
    }
    [SerializeField]
    public class Tester
    {
        public Tester(int id)
        {
            ID = id;
            modes = new List<List<float>>();
            PXI_M = new List<double>();
            PXI_I = new List<double>();
            IMI = new List<double>();
            danceFeel = new List<int>();
        }

        public int ID;

        public List<List<float>> modes;
        public List<double> PXI_M;
        public List<double> PXI_I;
        public List<double> IMI;
        public List<int> danceFeel;
    }
   
    
    [SerializeField]
    public class Testers
    {
        public List<Tester> testers;
    }
}
