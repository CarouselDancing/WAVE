using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrackBodyVariables : MonoBehaviour
{
    public Transform head;
    public Transform hand;

    public VisualEffect vfx;
    public VisualEffect vfx_burst;

    float speed_hand;
    float[] average_speed_hand;
    public int AverageCycleSize_speed_hand;

    float acceleration_hand;

    Vector3 dir_hand;
    Vector3[] average_dir_hand;
    public int AverageCycleSize_dir_hand;

    float dirTorso_hand;
    float[] average_dirTorso_hand;
    public int AverageCycleSize_dirTorso_hand;

    float rotSpeed_hand;

    Vector3 old_pos_hand;
    int frameCount;
    // Start is called before the first frame update
    void Start()
    {
        average_speed_hand = new float[AverageCycleSize_speed_hand];
        average_dir_hand = new Vector3[AverageCycleSize_dir_hand];
        average_dirTorso_hand = new float[AverageCycleSize_dirTorso_hand];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frameCount++;

        float old_speed_hand = average_speed_hand[(frameCount - 1) % AverageCycleSize_speed_hand];
        speed_hand = Vector3.Distance(old_pos_hand, hand.position)/Time.fixedDeltaTime;
        acceleration_hand = speed_hand - old_speed_hand;

        int frame;

        frame = frameCount % AverageCycleSize_speed_hand;
        average_speed_hand[frame] = speed_hand;

        frame = frameCount % AverageCycleSize_dir_hand;
        dir_hand = (hand.position - old_pos_hand).normalized;
        average_dir_hand[frame] = dir_hand;

        frame = frameCount % AverageCycleSize_dirTorso_hand;
        Vector3 torsoPos = head.position - -new Vector3(0, -0.25f, 0);
        dirTorso_hand = Vector3.Distance(torsoPos, hand.position) - Vector3.Distance(torsoPos, old_pos_hand);
        average_dirTorso_hand[frame] = dirTorso_hand;

        //Setting VFXs
        float averageS = getAverage(average_speed_hand);
        Vector3 averageD = getAverage(average_dir_hand);
        float sumDt = getSum(average_dirTorso_hand);

        if (speed_hand < 20)
            vfx.SetFloat("test", speed_hand);
        if(sumDt > 0.25f && averageS > 1 && averageS < 10 && Mathf.Abs( acceleration_hand) > 1 && acceleration_hand < 20)
        {
            vfx_burst.enabled = false;
            vfx_burst.enabled = true;
        }


        old_pos_hand = hand.position;

        
    }

    float getAverage(float[] array)
    {
        float sum = 0;
        for(int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum / array.Length;
    }

    float getSum(float[] array)
    {
        float sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum ;
    }

    Vector3 getAverage(Vector3[] array)
    {
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return new Vector3(sum.x / array.Length, sum.y / array.Length, sum.z / array.Length);
    }
}
 