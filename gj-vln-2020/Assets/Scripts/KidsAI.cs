using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KidsAI : MonoBehaviour
{
    public  Animator[] AllBreakables;
    public  Animator KidAnimator;
    int ArrayIndexNext = 0;
    private NavMeshAgent KidAgent;

    private float KidAgentAcceleration = 2f;
    private float KidAgentDeseleration = 5f;
    private float KidAgentCloseToDestination =1f;
    private bool LookingForDestination;

    private bool KidRunning = false;
    private bool kidDestroying = false;
    private Vector3 currentDestination;

    //looking if kid is not stuck
    float lastCheckedTime;
    Vector3 lastKnownPosition;
    public float CheckForMovementSeconds = 3f;
    public float MinMovement = 1f;

    // Start is called before the first frame update
    void Awake()
    {       
        //Allbreakables
        KidAgent = GetComponent<NavMeshAgent>();
        lastKnownPosition = transform.position;
        lastCheckedTime = Time.time;
        //LookingForDestination = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(currentDestination, transform.position));
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

        if((Time.time - lastCheckedTime) > CheckForMovementSeconds)
        {
            if((transform.position - lastKnownPosition).magnitude < MinMovement)
            {
                KidRunning = false;
                kidDestroying = false; //force a new destination
            }
        }
    }

    void RandomIndex()
    {
        int newIndex = Random.Range(0, AllBreakables.Length);

        while (AllBreakables[newIndex].gameObject.GetComponent<Breakables>().BreakStatus != 0) //only generate non-broken destinations
        {
            newIndex = Random.Range(0, AllBreakables.Length);
        }

        ArrayIndexNext = newIndex;
    }

    void AnimationArrived()
    {
        //choose between destroying animations
        AllBreakables[ArrayIndexNext].gameObject.GetComponent<Breakables>().Break();
        string tag = AllBreakables[ArrayIndexNext].gameObject.tag;
        if (tag == "Pushable")
        {
            AnimationPushing();
        }
        else
        {
            AnimationKicking();
        }
    }
    void AnimationRunning()
    {
        LookingForDestination = false;
        KidRunning = true;
        kidDestroying = false;
        KidAgent.updateRotation = true;
        currentDestination = AllBreakables[ArrayIndexNext].transform.position;
        KidAgent.SetDestination(currentDestination);
        KidAnimator.CrossFadeInFixedTime("Running", 0.3f);
        KidAgent.acceleration = (KidAgent.remainingDistance < KidAgentCloseToDestination) ? KidAgentDeseleration
                                : KidAgentAcceleration;
    }
    void AnimationPushing()
    {
        kidDestroying = true;
        KidRunning = false;
        KidAgent.updateRotation = false;
        KidAnimator.CrossFadeInFixedTime("kid_Pushing", 0.3f);
        KidAgent.acceleration = (KidAgent.remainingDistance <KidAgentCloseToDestination) ? KidAgentDeseleration
                                :KidAgentAcceleration;

        StartCoroutine(WaitForKidToDestroy());
    }
    void AnimationKicking()
    {
        kidDestroying = true;
        KidRunning = false;
        KidAgent.updateRotation = false;
        KidAnimator.CrossFadeInFixedTime("kid_Kicking", 0.3f);
        KidAgent.acceleration = (KidAgent.remainingDistance < KidAgentCloseToDestination) ? KidAgentDeseleration
                                : KidAgentAcceleration;

        StartCoroutine(WaitForKidToDestroy());
    }

    IEnumerator WaitForKidToDestroy()
    {
        yield return new WaitForSeconds(3);
        kidDestroying = false;
    }
}
