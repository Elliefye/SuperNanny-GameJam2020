using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KidsAI : MonoBehaviour
{
    public  Animator[] AllBreakables;
    public  Animator KidAnimator;
    int ArrayIndexNext;
    private NavMeshAgent KidAgent;

    private float KidAgentAcceleration = 2f;
    private float KidAgentDeseleration = 5f;
    private float KidAgentCloseToDestination =1f;
    private bool LookingForDestination;

    private bool KidRunning;
    private bool kidDestroying;
    private Vector3 currentDestination;

    // Start is called before the first frame update
    void Start()
    {       
        //Allbreakables
        ArrayIndexNext = 0;
        KidAgent = GetComponent<NavMeshAgent>();       
        //LookingForDestination = true;
        KidRunning = false;
        kidDestroying = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(currentDestination, transform.position));
         if (!KidRunning && !kidDestroying)
         {
            RandomIndex();
            AnimationRunning();
         }
        else if (!kidDestroying)
        {
            if (Vector3.Distance(currentDestination, transform.position) < KidAgent.stoppingDistance)
            {
                KidAgent.SetDestination(transform.position);
                KidAgent.updateRotation = false;
                AnimationArrived();               
            }
        }
        else if (!KidRunning)
        {
            StartCoroutine(WaitForKidToDestroy());
        }
    }

    void RandomIndex()
    {
        Debug.Log(AllBreakables[ArrayIndexNext].transform.position);

        int newIndex = Random.Range(0, AllBreakables.Length);

        while(newIndex == ArrayIndexNext) //prevent generating the same destination twice in a row
        {
            newIndex = Random.Range(0, AllBreakables.Length);
        }

        ArrayIndexNext = newIndex;
    }

    void AnimationArrived()
    {   
        Debug.Log("yaaas");        
        AnimationPushing();
    }
    void AnimationRunning()
    {
        LookingForDestination = false;
        KidRunning = true;
        kidDestroying = false;
        KidAgent.updateRotation = true;
        currentDestination = AllBreakables[ArrayIndexNext].transform.position;
        KidAgent.SetDestination(currentDestination);
        KidAnimator.CrossFadeInFixedTime("Running", 0f);
        KidAgent.acceleration = (KidAgent.remainingDistance < KidAgentCloseToDestination) ? KidAgentDeseleration
                                : KidAgentAcceleration;
    }
    void AnimationPushing()
    {
        kidDestroying = true;
        KidRunning = false;
        KidAgent.updateRotation = false;
        KidAnimator.CrossFadeInFixedTime("kid_Pushing", 0f);
        KidAgent.acceleration = (KidAgent.remainingDistance <KidAgentCloseToDestination) ? KidAgentDeseleration
                                :KidAgentAcceleration;
    }
    void AnimationKicking()
    {

    }

    IEnumerator WaitForKidToDestroy()
    {
        yield return new WaitForSeconds(3);
        kidDestroying = false;
    }
}
