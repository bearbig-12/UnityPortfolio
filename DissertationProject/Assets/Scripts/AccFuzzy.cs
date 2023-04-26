using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AccFuzzy : MonoBehaviour
{
    //Input 
    [SerializeField]
    public float distance;
    public float NumberOfobstacle;
    //Output
    public float AccessBility;


    public AnimationCurve distanceCloseMembership;
    public AnimationCurve distanceMiddleMembership;
    public AnimationCurve distanceFarMembership;

    public float distanceClose;
    public float distanceMiddle;
    public float distanceFar;

    public AnimationCurve obstacleLowMembership;
    public AnimationCurve obstacleMiddleMembership;
    public AnimationCurve obstacleHighMembership;

    public float ObsDensityLow;
    public float ObsDensityMiddle;
    public float ObsDensityHigh;


    GameObject player;
    public LayerMask ObstacleMask;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //Distance
        distance = Vector3.Distance(transform.position,player.transform.position);

        float distanceToPlayerClose = Remap(distance, 0f, 45f, 0f, 1f);
        float distanceToPlayerMiddle = DistanceMiddleSet(distance);
        float distanceToPlayerFar = Remap(distance, 55f, 100f, 0f, 1f);
        distanceClose = distanceCloseMembership.Evaluate(distanceToPlayerClose);
        distanceMiddle = distanceToPlayerMiddle;
        distanceFar = distanceFarMembership.Evaluate(distanceToPlayerFar);

        // Obstacles
        NumberOfobstacle = CheckingObstacles();
        
        float ObstaclesLow = Remap(NumberOfobstacle, 0.8f, 1.3f,0f,1f);
        float ObstaclesMiddle = OBSMiddleSet(NumberOfobstacle);
        float ObstaclesHigh = Remap(NumberOfobstacle, 1.7f, 3f, 0f, 1f);
        ObsDensityLow = obstacleLowMembership.Evaluate(ObstaclesLow);
        ObsDensityMiddle = ObstaclesMiddle;
        ObsDensityHigh = obstacleHighMembership.Evaluate(ObstaclesHigh);

        //Fuzzy Rule
        float Rule0 = distanceClose; // ACC G
        float Rule1 = Mathf.Min(distanceClose, ObsDensityHigh); //Acc M
        float Rule2 = Mathf.Min(distanceMiddle,ObsDensityLow); // Acc G
        float Rule3 = Mathf.Min(distanceMiddle,ObsDensityMiddle); // M
        float Rule4 = Mathf.Min(distanceMiddle, ObsDensityHigh); // Acc B
        //float Rule5 = Mathf.Min(distanceFar, ObsDensityLow); // Acc M
        float Rule6 = distanceFar; //Acc B

        //Aggregation the Output Membership
        float[] Aggregation_Outputs = new float[101];

        //Aggregation(Rule0, Rule1, Rule2, Rule3, Rule4, Rule5, Rule6, Aggregation_Outputs);
        Aggregation(Rule0, Rule1, Rule2, Rule3, Rule4, Rule6, Aggregation_Outputs);

        //Defuzzification & Accessbility
        AccessBility = Defuzzification(Aggregation_Outputs);
        //Debug.Log(AccessBility);
        //Invoke("Check", 2f);

    }
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    float DistanceMiddleSet(float distance)
    {
        float Left = 10f;
        float Middle = 50f;
        float Right = 90f;

        if (distance < Left || distance > Right)
        {
            return 0f;
        }
        else if (distance >= Left && distance <= Middle)
        {
            return (distance - Left) / (Middle - Left);
        }
        else if (distance > Middle && distance <= Right)
        {
            return (Right - distance) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }

    float CheckingObstacles()
    {
        //int obstacleCount = 0;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, distance, ObstacleMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            RaycastHit[] hits;

            if (hit.collider.tag == "Obstacle")
            {
                hits = Physics.RaycastAll(transform.position, player.transform.position - transform.position, distance, ObstacleMask);
                return hits.Length;
            }
          
        }
        Debug.DrawLine(transform.position, player.transform.position, Color.blue);


        //Debug.Log("Not Chcked");
        return 0f;
    }
    float OBSMiddleSet(float OBSs)
    {
        float Left = 0.3f;
        float Middle = 1.5f;
        float Right = 2.7f;

        if (OBSs < Left || OBSs > Right)
        {
            return 0f;
        }
        else if (OBSs >= Left && OBSs <= Middle)
        {
            return (OBSs - Left) / (Middle - Left);
        }
        else if (OBSs > Middle && OBSs <= Right)
        {
            return (Right - OBSs) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }
    
    float AccessbilityBad(float index)
    {
        return 1- Remap(index, 0, 45f, 0, 1f);
    }
    float AccessbilityMiddle(float index)
    {
        return AccMiddleSet(index);
    }
    float AccessbilityGood(float index)
    {
        return Remap(index, 55f, 100f, 0, 1f);
    }

    float AccMiddleSet(float Accs)
    {
        float Left = 10f;
        float Middle = 50f;
        float Right = 90f;


        if (Accs < Left || Accs > Right)
        {
            return 0f;
        }
        else if (Accs >= Left && Accs <= Middle)
        {
            return (Accs - Left) / (Middle - Left);
        }
        else if (Accs > Middle && Accs <= Right)
        {
            return (Right - Accs) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }

    float Defuzzification(float[] Aggregation_Outputs)
    {
        float numerator = 0;
        float denominator = 0;

        for (int i = 0; i <= 100; ++i)
        {
            numerator += Aggregation_Outputs[i] * i;
            denominator += Aggregation_Outputs[i];
        }
        return numerator / denominator;
        
    }
    //void Aggregation(float r0, float r1, float r2, float r3, float r4, float r5, float r6, float[] agg_Output)
    //{
    //    for (int i = 0; i <= 100; ++i)
    //    {
    //        float Rule0_ = Mathf.Min(r0, AccessbilityGood(i));
    //        float Rule1_ = Mathf.Min(r1, AccessbilityMiddle(i));
    //        float Rule2_ = Mathf.Min(r2, AccessbilityGood(i));
    //        float Rule3_ = Mathf.Min(r3, AccessbilityMiddle(i));
    //        float Rule4_ = Mathf.Min(r4, AccessbilityBad(i));
    //        float Rule5_ = Mathf.Min(r5, AccessbilityMiddle(i));
    //        float Rule6_ = Mathf.Min(r6, AccessbilityBad(i));

    //        agg_Output[i] = Mathf.Max(Rule0_, Rule1_, Rule2_, Rule3_, Rule4_, Rule5_, Rule6_);
    //    }
    //}

    void Aggregation(float r0, float r1, float r2, float r3, float r4, float r5, float[] agg_Output)
    {
        for (int i = 0; i <= 100; ++i)
        {
            float Rule0_ = Mathf.Min(r0, AccessbilityGood(i));
            float Rule1_ = Mathf.Min(r1, AccessbilityMiddle(i));
            float Rule2_ = Mathf.Min(r2, AccessbilityGood(i));
            float Rule3_ = Mathf.Min(r3, AccessbilityMiddle(i));
            float Rule4_ = Mathf.Min(r4, AccessbilityBad(i));
            float Rule5_ = Mathf.Min(r5, AccessbilityBad(i));

            agg_Output[i] = Mathf.Max(Rule0_, Rule1_, Rule2_, Rule3_, Rule4_, Rule5_);
        }
    }


    //void Check()
    //{
    //   // Debug.Log(distance);
    //    //Debug.Log(distanceMiddle);
    //}
}
