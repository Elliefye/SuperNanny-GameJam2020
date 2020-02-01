//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class KidsAI : MonoBehaviour
//{


//    public  Animator[] AllBreakables;
//    public  Animator KidAnimator;
//    int ArrayIndexNext;
//    private NavMeshAgent KidAgent;

//    private float KidAgentAcceleration = 2f;
//    private float KidAgentDeseleration = 5f;
//    private float KidAgentCloseToDestination =0.0000000000001f;
//    private bool LookingForDestination;

//    private bool KidRunning;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//        //Allbreakables
//        ArrayIndexNext = 0;
//        KidAgent = GetComponent<NavMeshAgent>();
//        RandomIndex();
//        //LookingForDestination = true;
//        KidRunning = false;
        
        
//    }

//    // Update is called once per frame
//    void Update()
//    {   
//        Debug.Log(KidAgent.remainingDistance + " " + LookingForDestination);
//         if (!KidRunning)
//         {
//            AnimationRunning();
//         }
        
        
        
        
//    }

//    void RandomIndex()
//    {
//        Debug.Log(AllBreakables[ArrayIndexNext].transform.position);
//        ArrayIndexNext = Random.Range(0, AllBreakables.Length);
        
//    }

//    void AnimationArrived()
//    {   Debug.Log("yaaas");
//        AnimationPushing();
//    }
//    void AnimationRunning()
//    {
//        LookingForDestination = false;
//        KidRunning =true;
//        KidAgent.updateRotation = true;
//        Vector3 Destination = KidAgent.SetDestination(AllBreakables[ArrayIndexNext].transform.position);
//        KidAnimator.CrossFadeInFixedTime("Running", 0f);
//        KidAgent.acceleration = (KidAgent.remainingDistance <KidAgentCloseToDestination) ? KidAgentDeseleration
//                                :KidAgentAcceleration;

//        if (Vector3.Distance(Destination, transform.position) < KidAgent.stoppingDistance)
//        {
//            KidAgent.SetDestination(transform.position);
//            KidAgent.updateRotation = false;
//            AnimationArrived();
//        }
//    }
//    void AnimationPushing()
//    {
//        KidAgent.updateRotation = false;
//        KidAnimator.CrossFadeInFixedTime("kid_Pushing", 0f);
//        KidAgent.acceleration = (KidAgent.remainingDistance <KidAgentCloseToDestination) ? KidAgentDeseleration
//                                :KidAgentAcceleration;
//    }
//    void AnimationKicking()
//    {

//    }
//}
