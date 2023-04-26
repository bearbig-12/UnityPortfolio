using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class FuzzyEnemyAI : MonoBehaviour
{
    public float Player_HP;
    public float AccessBility;
    public float WeaponRange;
    public float Relate_HP;

    public AnimationCurve accessbilityBadMembership;
    public AnimationCurve accessbilityMiddleMembership;
    public AnimationCurve accessbilityGoodMembership;

    float F_accessbilityBad;
    float F_accessbilityMiddle;
    float F_accessbilityGood;

    public AnimationCurve RelateHPLowMembership;
    public AnimationCurve RelateHPSimilarMembership;
    public AnimationCurve RelateHPHighMembership;

    float F_relateHPLow;
    float F_relateHPSimilar;
    float F_relateHPHigh;

    public AnimationCurve WeaponRangeMeleeMembership;
    public AnimationCurve WeaponRangeMiddleMembership;
    public AnimationCurve WeaponRangeLongMembership;

    float F_weaponRangeMelee;
    float F_weaponRangeMiddle;
    float F_weaponRangeLong;

    public AnimationCurve BehaviorPatrolMembership;
    public AnimationCurve BehaviorRunAwayMembership;
    public AnimationCurve BehaviorChaseMembership;
    public AnimationCurve BehaviorAttackMembership;

    public float F_behaviorPatrol;
    public float F_behaviorRunAway;
    public float F_behaviorChase;
    public float F_behaviorAttack;

    public float Output;

    float[] Behaviors = new float[4];
    public int BeHaviorType;
    bool pickTerm;

    public string Name;
    enum State
    {
        Patrol,
        Chase,
        Attack,
        RunAway,
        Die

    }

    State Current_State;

    enum WeaponType
    {
        Melee,
        Rifle,
        Sniper
    }
    WeaponType Current_Weapon;
    public int Weapon_Type;

    NavMeshAgent agent;
    public Transform target;
    GameObject player;

    Animator anim;


    bool isOnSight;

    public float HP;

    //Patrol
    public Transform[] WayPoints;
    int c_wayPoints;
    Vector3 TargetWaypoint;

    // Attack
    public float AttackTerm;
    bool Attacked;

    public float SightRange, AttackRange;

    public LayerMask PlayerMask;


    public GameObject Bullet;
    public Transform Bullet_Point;
    public float Bullet_Speed = 20000f;

    [SerializeField]
    private HeathBar HP_Bar;
    float Max_HP;

    // Start is called before the first frame update
    void Start()
    {
        Max_HP = 100;
        HP = 100;

        HP_Bar.UpdateHPBar(Max_HP, HP);

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        if (Weapon_Type == 0)
        {
            Current_Weapon = WeaponType.Melee;
            SightRange = 40;
            AttackRange = 10;
        }
        else if (Weapon_Type == 1)
        {
            Current_Weapon = WeaponType.Rifle;
            SightRange = 60;
            AttackRange = 40;
        }
        else
        {
            Current_Weapon = WeaponType.Sniper;
            SightRange = 100;
            AttackRange = 70;
        }
        Current_State = State.Patrol;
        UpdateWayPoints();

        isOnSight = false;
        Attacked = false;
        pickTerm = false;
    }

    // Update is called once per frame
    void Update()
    {
        HP_Bar.UpdateHPBar(Max_HP, HP);


        GameObject enemy = GameObject.Find(Name);
        AccFuzzy AccessbilityOutput = enemy.GetComponent<AccFuzzy>();

        //AccessBility
        AccessBility = AccessbilityOutput.AccessBility;

        float AccessbilityBad = Remap(AccessBility, 13f, 45f, 0f, 1f);
        float AccessbilityMiddle = AccessbilityMiddleSet(AccessBility);
        float AccessbilityGood = Remap(AccessBility, 55f, 85f, 0, 1f);
        F_accessbilityBad = accessbilityBadMembership.Evaluate(AccessbilityBad);
        F_accessbilityMiddle = AccessbilityMiddle;
        F_accessbilityGood = accessbilityGoodMembership.Evaluate(AccessbilityGood);

        //RelateHP
     
        Player_HP = player.GetComponent<PlayerMovement>().Player_HP;
 
        Relate_HP = HP - Player_HP;

        float RelateHPAgainstPlayerLower = Remap(Relate_HP, -100f, -10f, 0f, 1f);
        float RelateHPAgainstPlayerSimilar = RelateHPMiddleSet(Relate_HP);
        float RelateHPAgainstPlayerBigger = Remap(Relate_HP, 10f, 100f, 0f, 1f);
        F_relateHPLow = RelateHPLowMembership.Evaluate(RelateHPAgainstPlayerLower);
        F_relateHPSimilar = RelateHPAgainstPlayerSimilar;
        F_relateHPHigh = RelateHPHighMembership.Evaluate(RelateHPAgainstPlayerBigger);

        //Debug.Log(relateHPLow);
        //Debug.Log(F_relateHPSimilar.ToString("F6"));
        //Debug.Log(relateHPHigh);


        //Weapon Range
        WeaponRange = AttackRange;

        float WeaponRangeMelee = Remap(WeaponRange, 0f, 40f, 0f, 1f);
        float WeaponRangeMiddle = WeaponMiddleSet(WeaponRange);
        float WeaponRnageLong = Remap(WeaponRange, 60f, 100f, 0f, 1f);
        F_weaponRangeMelee = WeaponRangeMeleeMembership.Evaluate(WeaponRangeMelee);
        F_weaponRangeMiddle = WeaponRangeMiddle;
        F_weaponRangeLong = WeaponRangeLongMembership.Evaluate(WeaponRnageLong);


        //Fuzzy Rule
        float R1 = Mathf.Min(F_accessbilityBad, F_weaponRangeMelee); // P
        float R2 = Mathf.Min(F_accessbilityBad, F_relateHPLow, F_weaponRangeMiddle); // C
        float R3 = Mathf.Min(F_accessbilityBad, F_relateHPLow, F_weaponRangeLong); // A
        float R4 = Mathf.Min(F_accessbilityBad, F_relateHPSimilar, F_weaponRangeMiddle); // P
        float R5 = Mathf.Min(F_accessbilityBad, F_relateHPSimilar, F_weaponRangeLong); // P
        float R6 = Mathf.Min(F_accessbilityBad, F_relateHPHigh,F_weaponRangeMiddle); // P
        float R7 = Mathf.Min(F_accessbilityBad, F_relateHPHigh, F_weaponRangeLong); // P
        float R8 = Mathf.Min(F_accessbilityMiddle, F_relateHPLow, F_weaponRangeMelee); // C
        float R9 = Mathf.Min(F_accessbilityMiddle, F_relateHPLow, F_weaponRangeMiddle); // A
        float R10 = Mathf.Min(F_accessbilityMiddle, F_weaponRangeLong); // A
        float R11 = Mathf.Min(F_accessbilityMiddle, F_relateHPSimilar, F_weaponRangeMelee); // p
        float R12 = Mathf.Min(F_accessbilityMiddle, F_relateHPSimilar, F_weaponRangeMiddle); // c
        float R13 = Mathf.Min(F_accessbilityMiddle, F_relateHPHigh, F_weaponRangeMelee); // p
        float R14 = Mathf.Min(F_accessbilityMiddle, F_relateHPHigh, F_weaponRangeMiddle); // C
        float R15 = Mathf.Min(F_accessbilityGood, F_relateHPLow, F_weaponRangeMelee); // A
        float R16 = Mathf.Min(F_accessbilityGood, F_relateHPLow, F_weaponRangeMiddle); // R
        float R17 = Mathf.Min(F_accessbilityGood, F_relateHPLow, F_weaponRangeLong); // R
        float R18 = Mathf.Min(F_accessbilityGood, F_relateHPSimilar); // A
        float R19 = Mathf.Min(F_accessbilityGood, F_relateHPHigh); // A


        //Aggregation the Output Membership
        float[] Aggregation_Outputs = new float[101];

        Aggregation(R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15, R16, R17, R18, R19, Aggregation_Outputs);
        Output = Defuzzification(Aggregation_Outputs);

        float OutputPatrol = Remap(Output, 0f, 30f, 0f, 1f);
        float OutputRunAway = RunAwayMiddleSet(Output);
        float OutputChase = ChaseMiddleSet(Output);
        float OutputAttack = Remap(Output, 70f,100f,0f, 1f);

        F_behaviorPatrol = BehaviorPatrolMembership.Evaluate(OutputPatrol);
        F_behaviorRunAway = OutputRunAway;
        F_behaviorChase = OutputChase;
        F_behaviorAttack = BehaviorAttackMembership.Evaluate(OutputAttack);

        
        Behaviors[0] = F_behaviorPatrol;
        Behaviors[1] = F_behaviorRunAway;
        Behaviors[2] = F_behaviorChase;
        Behaviors[3] = F_behaviorAttack;


        PickedBehaviorType();

        if (BeHaviorType == 0)
        {
            //Patrol
            anim.SetBool("isPatroling", true);
            anim.SetBool("isChasing", false);
            anim.SetBool("isShooting", false);
            agent.speed = 10f;
            if (agent.remainingDistance < 7f)
            {
                //Debug.Log("Agent is Stopped");
                UpdateWayPoints();
                Patrol();
            }
            Patrol();

        }
        else if(BeHaviorType == 1)
        {
            //RunAway
            agent.speed = 50f;
            RunAway();
       

        }
        else if(BeHaviorType == 2) 
        {
            if (Current_Weapon == WeaponType.Melee)
            {
                agent.speed = 40f;
            }
            else
            {
                agent.speed = 25f;
            }
            //Chase
            anim.SetBool("isChasing", true);
            anim.SetBool("isPatroling", false);
            anim.SetBool("isShooting", false);
            Chase();
        }
        else
        {
            if (Current_Weapon == WeaponType.Melee)
            {
                agent.speed = 40f;
            }
            else
            {
                agent.speed = 25f;
            }
            //Attack
            Attack();
        }
        
    }

    void Patrol()
    {

        
        agent.SetDestination(WayPoints[c_wayPoints].position);
    }

    void UpdateWayPoints()
    {
        int index = Random.Range(0, WayPoints.Length);
        c_wayPoints = index;
        TargetWaypoint = WayPoints[c_wayPoints].position;
    }
    void Chase()
    {
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        agent.SetDestination(target.position);
        if (Attacked == false)
        {
            if (Current_Weapon == 0)
            {
                anim.SetBool("isChasing", false);
                anim.SetBool("isPatroling", false);
                anim.SetBool("isShooting", true);
            }
            else
            {
                anim.SetBool("isChasing", false);
                anim.SetBool("isPatroling", false);
                anim.SetBool("isShooting", true);
                GameObject bullet = Instantiate(Bullet, Bullet_Point.transform.position, Bullet_Point.transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * Bullet_Speed);
                Destroy(bullet, 1);
                Attacked = true;
                Invoke("NextAttackTerm", 1.5f);
            }

        }

    }


    public void DamageTake(float damage)
    {
        HP -= damage;

    }
    public void Die()
    {
        agent.isStopped = true;


    }

    void RunAway()
    {
        anim.SetBool("isChasing", true);
        anim.SetBool("isPatroling", false);
        anim.SetBool("isShooting", false);
        Vector3 dirToPlayer = transform.position - player.transform.position;

        Vector3 RunAwayPos = transform.position + dirToPlayer;

        agent.SetDestination(RunAwayPos);
    }

    void C_HP()
    {
        Debug.Log(HP);
    }
    void NextAttackTerm()
    {
        Attacked = false;
    }


    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    float RelateHPMiddleSet(float relateHP)
    {
        float Left = -80f;
        float Middle = 0f;
        float Right = 80f;

        if (relateHP == Middle)
        {
            return 1f;
        }
        if (relateHP < Left || relateHP > Right)
        {
            return 0f;
        }
        else if (relateHP >= Left && relateHP <= Middle)
        {
            return (relateHP - Left) / (Middle - Left);
        }
        else if (relateHP > Middle && relateHP <= Right)
        {
            return (Right - relateHP) / (Right - Middle);
        }
        else
        {
            return 0f;
        }
    }

    float AccessbilityMiddleSet(float acc_)
    {
        float Left = 20f;
        float Middle = 50f;
        float Right = 80f;

        if (acc_ < Left || acc_ > Right)
        {
            return 0f;
        }
        else if (acc_ >= Left && acc_ <= Middle)
        {
            return (acc_ - Left) / (Middle - Left);
        }
        else if (acc_ > Middle && acc_ <= Right)
        {
            return (Right - acc_) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }
    float WeaponMiddleSet(float range)
    {
        float Left = 10f;
        float Middle = 50f;
        float Right = 90f;

        if (range < Left || range > Right)
        {
            return 0f;
        }
        else if (range >= Left && range <= Middle)
        {
            return (range - Left) / (Middle - Left);
        }
        else if (range > Middle && range <= Right)
        {
            return (Right - range) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }
    float BehaviorPatrol(float index)
    {
        return 1 - Remap(index, 0, 30f, 0, 1f);

    }
    float BehaviorRunAway(float index)
    {
        return RunAwayMiddleSet(index);
    }
    float BehaviorChase(float index)
    {
        return ChaseMiddleSet(index);
    }
    float BehaviorAttackl(float index)
    {
        return Remap(index, 70, 100f, 0, 1f);
    }
    float RunAwayMiddleSet(float index)
    {
        float Left = 10f;
        float Middle = 35f;
        float Right = 60f;

     
        if (index < Left || index > Right)
        {
            return 0f;
        }
        else if (index >= Left && index <= Middle)
        {
            return (index - Left) / (Middle - Left);
        }
        else if (index > Middle && index <= Right)
        {
            return (Right - index) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }
    float ChaseMiddleSet(float index)
    {
        float Left = 40f;
        float Middle = 65f;
        float Right = 90f;

      
        if (index < Left || index > Right)
        {
            return 0f;
        }
        else if (index >= Left && index <= Middle)
        {
            return (index - Left) / (Middle - Left);
        }
        else if (index > Middle && index <= Right)
        {
            return (Right - index) / (Right - Middle);
        }
        else
        {
            return 0f;
        }

    }

    void Aggregation(float r1, float r2, float r3, float r4, float r5, float r6, float r7, 
        float r8, float r9 , float r10, float r11 ,float r12, float r13, float r14, float r15, 
        float r16, float r17, float r18, float r19, float[] agg_Output)
    {
        for (int i = 0; i <= 100; ++i)
        {
            float Rule1_ = Mathf.Min(r1, BehaviorPatrol(i));
            float Rule2_ = Mathf.Min(r2, BehaviorChase(i));
            float Rule3_ = Mathf.Min(r3, BehaviorAttackl(i));
            float Rule4_ = Mathf.Min(r4, BehaviorPatrol(i));
            float Rule5_ = Mathf.Min(r5, BehaviorPatrol(i));
            float Rule6_ = Mathf.Min(r6, BehaviorPatrol(i));
            float Rule7_ = Mathf.Min(r7, BehaviorPatrol(i));
            float Rule8_ = Mathf.Min(r8, BehaviorChase(i));
            float Rule9_ = Mathf.Min(r9, BehaviorAttackl(i));
            float Rule10_ = Mathf.Min(r10, BehaviorAttackl(i));
            float Rule11_ = Mathf.Min(r11, BehaviorPatrol(i));
            float Rule12_ = Mathf.Min(r12, BehaviorChase(i));
            float Rule13_ = Mathf.Min(r13, BehaviorPatrol(i));
            float Rule14_ = Mathf.Min(r14, BehaviorChase(i));
            float Rule15_ = Mathf.Min(r15, BehaviorAttackl(i));
            float Rule16_ = Mathf.Min(r16, BehaviorRunAway(i));
            float Rule17_ = Mathf.Min(r17, BehaviorRunAway(i));
            float Rule18_ = Mathf.Min(r18, BehaviorAttackl(i));
            float Rule19_ = Mathf.Min(r19, BehaviorAttackl(i));

            agg_Output[i] = 
                Mathf.Max(Rule1_, Rule2_, Rule3_, Rule4_, Rule5_, Rule6_, 
                Rule7_, Rule8_, Rule9_, Rule10_, Rule11_, Rule12_, Rule13_, Rule14_, 
                Rule15_, Rule16_, Rule17_, Rule18_, Rule19_);
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
    void PickedBehaviorType()
    {
        if(pickTerm == false)
        {
            BeHaviorType = PickBehavior(Behaviors);
            pickTerm = true;
            Invoke("NextPickTerm", 1.0f);
        }
    }
    void NextPickTerm()
    {
        pickTerm = false;
    }
    int PickBehavior(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
  

}
