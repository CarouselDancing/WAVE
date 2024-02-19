using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceQueue : MonoBehaviour
{
    List<Material> materials;
    public Material M;
    public GameObject DancerPrefab;
    public GameObject DancerPrefab2;
    [SerializeField] Transform player;
    [SerializeField] Transform teacher;
    [Range(0,25)] public int numberOfDancers;
    BeatPlayer beatPlayer;
    string animName;
    float AnimLength;
    List<Coroutine> coroutines;

    public Shader shader;

    public List<Material> mmm;
    // Start is called before the first frame update
    void Awake()
    {
        coroutines = new List<Coroutine>();
        materials = new List<Material>();
        //beatPlayer = GameObject.FindObjectOfType<BeatPlayer>();
        ////beatPlayer.danceQ = this;
        //player = beatPlayer.Player;
    }

    // Update is called once per frame
    void Update()
    {
       // print(beatPlayer.AnimTime());

    }

    public void addQue(int index)
    {
        if(transform.childCount < 6)
        {
            Instantiate(new GameObject(), transform);
            Instantiate(new GameObject(), transform);
            Instantiate(new GameObject(), transform);
            Instantiate(new GameObject(), transform);
            Instantiate(new GameObject(), transform);
            Instantiate(new GameObject(), transform);
        }
        UpdateAmount(index);
    }

    public void UpdateAmount(int index)
    {
        var bezi = beatPlayer.CURVES.GetChild(index).GetComponent<Bezier>();
        bezi.updatePoints();

        //while (materials.Count < bezi.Amount())
        //{
        //    materials.Add(new Material(M));
        //}

        //for (int i = 0; i < materials.Count; i++)
        //{
        //    materials[i].SetColor("Color_91769470b91d46dd840f1cba08391e4e",
        //        Color.Lerp(beatPlayer.formation.startColor, beatPlayer.formation.startColor, i * 0.1f / bezi.Amount()));
        //}

        int e = 0;
        
        while (bezi.Amount() > transform.GetChild(index*2).childCount)
        {
            
            e++;
            if (e > 100)
            {
                print("ERROR");
                break;
            }

            GameObject g;
            Material m = mmm[e];
            float t = 3;
            float sv = -1f;
            float ev = 0.5f;
            t += e;
            if (index == 0)
            {
                 g = Instantiate(DancerPrefab, Vector3.zero, teacher.rotation, transform.GetChild(0));
                 
                 g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = m;
                 m.SetFloat("_alpha", sv);
                 m.DOFloat(ev, "_alpha", t).SetEase(Ease.InExpo);
                 //g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = materials[g.transform.GetSiblingIndex()];
            }
            else
            {
                g = Instantiate(DancerPrefab2, Vector3.zero, teacher.rotation, transform.GetChild(index * 2));
                //g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = materials[g.transform.GetSiblingIndex()];
                g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = m;
                //m.SetFloat("_alpha", -0.5f);
                //m.DOFloat(0.5f, "_alpha", 3f);

                g = Instantiate(DancerPrefab2, Vector3.zero, teacher.rotation, transform.GetChild(index * 2 +1));
                g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = m;
                m.SetFloat("_alpha", sv);
                m.DOFloat(ev, "_alpha", t).SetEase(Ease.InExpo);

                //g.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = materials[g.transform.GetSiblingIndex()];

            }

           
        }

        while (bezi.Amount() < transform.GetChild(index * 2).childCount)
        {
            e++;
            if (e > 100)
            {
                print("ERROR");
                break;
            }
          
                var g = transform.GetChild(index*2).GetChild(0);
                g.parent = null;
                Destroy(g.gameObject);

            if (index != 0) 
            { 
                g = transform.GetChild(index*2+1).GetChild(0);
                g.parent = null;
                Destroy(g.gameObject);

            }
        }



        int count = transform.GetChild(index * 2).childCount;

        for (int i = 0; i < count; i++)
        {
            float scale = teacher.localScale.x + (player.localScale.x * beatPlayer.teacherScale - teacher.localScale.x) / (count + 1) * i;
            transform.GetChild(index * 2).GetChild(i).localScale = Vector3.one * scale;
            if(index != 0)
                transform.GetChild(index * 2 +1 ).GetChild(i).localScale = Vector3.one * scale;
        }

        UpdatePositions(index);
        
    }

    public void UpdatePositions(int index)
    {
        Vector3 v = beatPlayer.CURVES.GetChild(0).GetChild(0).position;
        teacher.parent.position = new Vector3(v.x, 0, v.z);

        var bezi = beatPlayer.CURVES.GetChild(index).GetComponent<Bezier>();
        bezi.updatePoints();

        for (int i = 0; i < bezi.points.Length; i++)
        {
            transform.GetChild(index*2).GetChild(i).position = bezi.points[i];
            if (index != 0)
                transform.GetChild(index * 2 +1 ).GetChild(i).position = new Vector3(bezi.points[i].x * -1, 0, bezi.points[i].z) ;

        }
    }


    public void UpdateAnimations()
    {
        StopAllCoroutines();
        coroutines.Clear();

        for (int index = 0; index < 3; index++) 
        {
            float t = beatPlayer.AnimTime();
            int beatsToT = beatPlayer.CURVES.GetChild(index).GetComponent<Bezier>().speed;
            int offsets = index == 0 ? transform.GetChild(index * 2).childCount + 1 : transform.GetChild(index * 2).childCount;
            float offset = (1.0f * beatPlayer.tempo * beatsToT) / offsets;

            print("INDEx " + index + " : childs: " + (transform.GetChild(index * 2).childCount) + "OFFSET ON _> " + offset);
            
            for (int i = 0; i < transform.GetChild(index * 2).childCount; i++)
            {
                
                coroutines.Add(StartCoroutine(_startAnimation(t - offset * (i + 1), transform.GetChild(index * 2).GetChild(i))));
                if (index != 0)
                    coroutines.Add(StartCoroutine(_startAnimation(t - offset * (i + 1), transform.GetChild(index * 2 + 1).GetChild(i))));

            }

        }

        float t_ = beatPlayer.AnimTime();
        int beatsToT_ = beatPlayer.CURVES.GetChild(0).GetComponent<Bezier>().speed;

        float offset_ = (1.0f * beatPlayer.tempo * beatsToT_) / (transform.GetChild(0).childCount + 1);
        coroutines.Add(StartCoroutine(_startShadow()));//StartCoroutine(_startAnimation(t_ - offset_ * (transform.GetChild(0).childCount + 1), beatPlayer.playerShadow.transform, false))); //Play(animName, 0, beatPlayer.tempo * beatPlayer.CURVES.GetChild(0).GetComponent<Bezier>().speed / AnimLength);


    }

    IEnumerator _startShadow()
    {
        
        beatPlayer.playerShadow.enabled = false;
        yield return new WaitForSeconds(beatPlayer.tempo * beatPlayer.CURVES.GetChild(0).GetComponent<Bezier>().speed);// / AnimLength);
        beatPlayer.playerShadow.enabled = true;

        beatPlayer.videoCharacter.position = beatPlayer.playerShadow.transform.position;
        beatPlayer.videoCharacter.localScale = beatPlayer.playerShadow.transform.localScale;
        beatPlayer.videoCharacter.GetComponent<Animator>().Play(animName, 0, 0);
        beatPlayer.playerShadow.Play(animName, 0);//, 0);



    }

    IEnumerator _startAnimation(float animTime, Transform g , bool showMesh = true)
    {
        if (showMesh)
        {
            //g.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            g.GetChild(0).localPosition = Vector3.zero;
        }
        g.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;

        var animator = g.GetChild(0).GetComponent<Animator>();
        //animator.Play(animName, 0, 0.0001f / AnimLength);
        animator.enabled = false;
        
        float time = animTime;

        while(time < 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

       



        

        animator.enabled = true;

        print("time " + time  + " anim: " +  AnimLength);

        animator.Play(animName, 0); //,  time / AnimLength);

        time = 0;

    
        yield return null;
        g.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    float GetAnimLength()
    {
        var clips = DancerPrefab.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController.animationClips;
        float L = 0;
        foreach (var clip in clips)
        {
            print("clip " + clip.name + " anim: " + animName);
            if (clip.name == animName)
            {
                L = clip.length;
                break;
            }
        }
        return L;
    }

    public void StartDancers(string animationName, BeatPlayer beatPlayer_)
    {
        animName = animationName;
        beatPlayer = beatPlayer_;
        player = beatPlayer.Player;

        AnimLength = GetAnimLength();
        addQue(0);
        addQue(1);
        addQue(2);
        //addQue(1);

        beatPlayer_.formation.changePreset();
        //UpdateAnimations();
        //StartCoroutine(test(animationName, beatPlayer_));
        //StartDancers_offset(animationName, beatPlayer, Vector3.zero);

        //StartDancers_offset(animationName, beatPlayer, Vector3.left * 2, true);
        //StartDancers_offset(animationName, beatPlayer, Vector3.right * 2, true);
    }

    //IEnumerator test(string animationName, BeatPlayer beatPlayer_)
    //{
        
    //}

    public void StartDancers_offset(string animationName, BeatPlayer beatPlayer, Vector3 offset, bool playerShadow = false)
    {
        player = beatPlayer.Player;

        float dist = Vector3.Distance(player.position, teacher.position) / (numberOfDancers + 1);

        int number = playerShadow ? numberOfDancers + 1 : numberOfDancers;
        for (int i = 1; i <= number; i++)
        {
            Vector3 pos = teacher.position + (player.position - teacher.position).normalized * (i * dist) + offset;

            var g = playerShadow ? Instantiate(DancerPrefab2, pos, teacher.rotation, transform) : Instantiate(DancerPrefab, pos, teacher.rotation, transform);
            float scale = teacher.localScale.x + (player.localScale.x * beatPlayer.teacherScale - teacher.localScale.x) / (numberOfDancers + 1) * i;

            g.transform.localScale = Vector3.zero;
            float waitTime = ((beatPlayer.tempo * beatPlayer.beatsToTarget) / (numberOfDancers + 1)) * i;

            StartCoroutine(startAnimation(waitTime, g.GetComponent<Animator>(), animationName, i == numberOfDancers, beatPlayer, g.transform, scale));
        }

        
    }
    IEnumerator startAnimation(float waitTime, Animator animator, string animationName, bool set_CanPause, BeatPlayer beatPlayer, Transform t, float scale)
    {
        yield return new WaitForSeconds(waitTime);
        t.localScale = Vector3.one * scale;
        //t.DOScale(scale, 0.3f);
        animator.enabled = true;
        animator.Play(animationName);
        if (set_CanPause)
            beatPlayer.setCanPause(true);
    }

    public void SetDancersSpeed(float speed)
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().speed = speed;
        }
    }

    public bool HideDancers(bool hide)
    {
        for (int i = 0; i < numberOfDancers; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = hide;
        }
        return hide;
    }
}
